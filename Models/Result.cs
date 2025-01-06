using System.Text.Json.Serialization;

namespace MADRSApp.Models
{
    public class Result
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("severity")]        
        public string? Severity { get; set; }
    }
}
