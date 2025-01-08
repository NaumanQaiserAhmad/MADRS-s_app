using System.Text.Json.Serialization;
using System.ComponentModel;

namespace MADRSApp.Models
{
    public class Question
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("rsp-1")]
        public AnswerOption? Rsp1 { get; set; }

        [JsonPropertyName("rsp-2")]
        public AnswerOption? Rsp2 { get; set; }

        [JsonPropertyName("rsp-3")]
        public AnswerOption? Rsp3 { get; set; }

        [JsonPropertyName("rsp-4")]
        public AnswerOption? Rsp4 { get; set; }
    }

    public class AnswerOption : INotifyPropertyChanged
    {
        private bool _isSelected;

        [JsonPropertyName("text")]

        public string? Text { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
