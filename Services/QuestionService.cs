using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MADRSApp.Models;

namespace MADRSApp.Services
{
    public class QuestionService
    {
        private const string BaseUrl = "https://flowns-app-test.herokuapp.com/api/";
        private readonly HttpClient _httpClient;

        public QuestionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Fetch a question by its number.
        /// </summary>
        /// <param name="questionNumber">The question number to fetch.</param>
        /// <returns>A Question object containing the title, text, and response options.</returns>
        public async Task<Question> GetQuestionAsync(int questionNumber)
        {
            try
            {
                string url = $"{BaseUrl}question/{questionNumber}/";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch question {questionNumber}: {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var question = JsonSerializer.Deserialize<Question>(json);

                return question ?? throw new InvalidOperationException("Received null or invalid question response from the API.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetQuestionAsync: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Submit answers and get the total score and severity.
        /// </summary>
        /// <param name="answers">Array of integers representing answers to the questions.</param>
        /// <returns>A Result object containing the total score and severity.</returns>
        public async Task<Result> SubmitAnswersAsync(int[] answers)
        {
            try
            {
                string url = $"{BaseUrl}answer/";
                var payload = new { answers };
                var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to submit answers: {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Result>(json);

                return result ?? throw new InvalidOperationException("Received null or invalid result response from the API.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SubmitAnswersAsync: {ex.Message}");
                throw;
            }
        }
    }
}
