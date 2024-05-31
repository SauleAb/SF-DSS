using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SF_DSS.Data.Entities;
using SF_DSS.Data;

namespace SF_DSS.Models.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly HttpClient _httpClient;
        private Conversation _conversation;
        private ApplicationDbContext _context;

        public ChatbotService(HttpClient httpClient, ApplicationDbContext context)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _conversation = new Conversation();
            _context = context;
        }

        public async Task<string> GetResponse(string message)
        {
            try
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(1000);
                var response = await _httpClient.GetAsync("http://95.99.2.118:5001/chat/" + message);
                response.EnsureSuccessStatusCode();

                var responseBody = response.Content.ReadAsStringAsync().Result;
                _context.Messages.Add(new Message{ Role = "user", MessageContent = message });
                _context.Messages.Add(new Message{ Role = "assistant", MessageContent = responseBody });
                await _context.SaveChangesAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                return "An error occurred while communicating with the chatbot API: " + ex.Message;
            }
        } 
         
        public Conversation GetConversation()
        {
            return _conversation;
        }

        public async Task SaveConversationAsync()
        {
            _context.Conversations.Add(_conversation);
            await _context.SaveChangesAsync();
        }
    }
}
