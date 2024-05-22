using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SF_DSS.Data.Entities;

namespace SF_DSS.Models.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly HttpClient _httpClient;

        public ChatbotService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetResponse(string message)
        {
            try
            {
                _httpClient.Timeout = new TimeSpan(1000000000);
                using (var response = _httpClient.GetAsync("http://145.93.168.223:5001/chat?message=" + message).Result)
                {
                    response.EnsureSuccessStatusCode();

                    var responseBody = response.Content.ReadAsStringAsync().Result;

                    return responseBody;
                }

                // var requestBody = JsonConvert.SerializeObject(new { crop });
                // 
                // var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                // 
                // using (var response = _httpClient.GetAsync("http://145.93.168.159:5000/inference?crop=" + crop).Result)
                // {
                //     response.EnsureSuccessStatusCode();
                // 
                //     var responseBody = await response.Content.ReadAsStringAsync();
                // 
                //     CropGrowthPlan jsonResponse = JsonConvert.DeserializeObject<CropGrowthPlan>(responseBody);
                // 
                //     return jsonResponse.ToFormattedString();
                // }
            }
            catch (Exception ex)
            {
                return "An error occurred while communicating with the chatbot API: " + ex.Message;
            }
        }
    }
}
