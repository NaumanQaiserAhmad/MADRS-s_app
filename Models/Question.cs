using System.Text.Json.Serialization;

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

    public class AnswerOption
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
