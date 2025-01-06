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

        public ObservableCollection<AnswerOption> Options { get; } = new();
        public ICommand LoadNextQuestionCommand { get; }
        public ICommand LoadPreviousQuestionCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel(QuestionService questionService)
        {
            _questionService = questionService;
            LoadNextQuestionCommand = new Command(async () => await LoadNextQuestionAsync(), () => CanGoNext);
            LoadPreviousQuestionCommand = new Command(async () => await LoadPreviousQuestionAsync(), () => CanGoBack);
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

        public async Task LoadNextQuestionAsync()
        {
            CurrentQuestionNumber++;
            await LoadQuestionAsync();
        }

        public async Task LoadPreviousQuestionAsync()
        {
            if (CurrentQuestionNumber > 1)
            {
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

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
