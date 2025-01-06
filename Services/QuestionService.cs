using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
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

        public async Task<Question> GetQuestionAsync(int questionNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                string url = $"{BaseUrl}question/{questionNumber}/";
                var response = await _httpClient.GetAsync(url, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch question {questionNumber}: {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                return JsonSerializer.Deserialize<Question>(json)
                    ?? throw new InvalidOperationException("Received null or invalid question response from the API.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetQuestionAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Result> SubmitAnswersAsync(int[] answers, CancellationToken cancellationToken = default)
        {
            try
            {
                string url = $"{BaseUrl}answer/";
                var payload = new { answers };
                var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                Console.WriteLine($"Sending payload: {JsonSerializer.Serialize(payload)}");

                var response = await _httpClient.PostAsync(url, jsonContent, cancellationToken);
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to submit answers: {response.StatusCode} - {responseContent}");
                }

                return JsonSerializer.Deserialize<Result>(responseContent)
                    ?? throw new InvalidOperationException("Received null or invalid result response from the API.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SubmitAnswersAsync: {ex.Message}");
                throw;
            }
        }
    }
}
