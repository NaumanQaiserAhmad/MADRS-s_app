using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MADRSApp.Models;
using MADRSApp.Services;

namespace MADRSApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly QuestionService _questionService;
        private int _currentQuestionNumber = 1;
        private Question? _currentQuestion;
        private AnswerOption? _selectedOption;
        private List<int> _answers = new();

        public ObservableCollection<AnswerOption> Options { get; } = new();
        public ICommand LoadNextQuestionCommand { get; }
        public ICommand LoadPreviousQuestionCommand { get; }
        public ICommand SubmitAnswersCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel(QuestionService questionService)
        {
            _questionService = questionService;

            LoadNextQuestionCommand = new Command(async () => await LoadNextQuestionAsync(), () => CanGoNext);
            LoadPreviousQuestionCommand = new Command(async () => await LoadPreviousQuestionAsync(), () => CanGoBack);
            SubmitAnswersCommand = new Command(async () => await SubmitAnswersAsync(), () => CanSubmit);

            LoadQuestionAsync().ConfigureAwait(false);
        }

        public Question? CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }

        public AnswerOption? SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged();
                (LoadNextQuestionCommand as Command)?.ChangeCanExecute();
            }
        }

        public int CurrentQuestionNumber
        {
            get => _currentQuestionNumber;
            set
            {
                if (_currentQuestionNumber != value)
                {
                    _currentQuestionNumber = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanGoBack));
                }
            }
        }

        public bool CanGoBack => _currentQuestionNumber > 1;

        public bool CanGoNext => SelectedOption != null;

        public bool CanSubmit => _answers.Count == 9;

        public async Task LoadNextQuestionAsync()
        {
            if (SelectedOption != null)
            {
                _answers.Add(Options.IndexOf(SelectedOption));

                if (_currentQuestionNumber == 9)
                {
                    OnPropertyChanged(nameof(CanSubmit));
                    return;
                }

                CurrentQuestionNumber++;
                await LoadQuestionAsync();
            }
        }

        public async Task LoadPreviousQuestionAsync()
        {
            if (_currentQuestionNumber > 1)
            {
                _answers.RemoveAt(_answers.Count - 1);
                CurrentQuestionNumber--;
                await LoadQuestionAsync();
            }
        }

        private async Task LoadQuestionAsync()
        {
            try
            {
                var question = await _questionService.GetQuestionAsync(CurrentQuestionNumber);

                CurrentQuestion = question;
                Options.Clear();
                if (question.Rsp1 != null) Options.Add(question.Rsp1);
                if (question.Rsp2 != null) Options.Add(question.Rsp2);
                if (question.Rsp3 != null) Options.Add(question.Rsp3);
                if (question.Rsp4 != null) Options.Add(question.Rsp4);

                SelectedOption = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading question: {ex.Message}");
            }
        }

        public async Task SubmitAnswersAsync()
        {
            try
            {
                var result = await _questionService.SubmitAnswersAsync(_answers.ToArray());
                await Application.Current.MainPage.DisplayAlert("Result",
                    $"Total Score: {result.Total}\nSeverity: {result.Severity}", "OK");

                ResetForm();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error submitting answers: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to calculate results. Try again.", "OK");
            }
        }

        private void ResetForm()
        {
            _currentQuestionNumber = 1;
            _answers.Clear();
            OnPropertyChanged(nameof(CanSubmit));
            LoadQuestionAsync().ConfigureAwait(false);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
