using SF_DSS.Data;
using SF_DSS.Data.Entities;

namespace SF_DSS.Models.Services
{
    public class JSONService
    {
        #region PRIVATE FIELDS
        private readonly HttpClient _httpClient;
        #endregion

        public JSONService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GreenhouseController> GetResponse(Conversation conversation)
        {
            try
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(100000);

                var response = await _httpClient.PostAsJsonAsync("http://95.99.2.118:5001/json_from_chat_history/", conversation.Messages);

                return await response.Content.ReadFromJsonAsync<GreenhouseController>();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public struct GreenhouseController
    {
        public string Crop { get; set; }
        public int Temperature { get; set; }
        public int Humidity { get; set; }
        public float Soil_ph { get; set; }
    }
}
