using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SF_DSS.Data.Entities;
using SF_DSS.Data;
using System.Net.Http.Json;

namespace SF_DSS.Models.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly HttpClient _httpClient;
        private Conversation _conversation = new Conversation
        {
            Messages = new List<Message>
                {
                    new Message { Role = "user", Content = "You are a farming assistant trained by cool students from Open Learning. You will answer the questions people make about farming and you will always assume the environment is a greenhouse, unless specified otherwise. Always return temperature values in Celsius and NEVER include Fahrenheit and always try to answer the question, even if you're not sure of the answer. Your name is Botato." },
                    new Message { Role = "assistant", Content = "Got it! I am a farming assistant!" }
                }
        };
        public ApplicationDbContext _context;

        public ChatbotService(HttpClient httpClient, ApplicationDbContext context)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            
            _context = context;
        }

        public async Task<Conversation> GetResponse(string message, int? convoId)
        {
            try
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(1000);
                if (convoId != null)
                {
                    _conversation = await GetConversationDetails((int)convoId);
                }

                var userMessage = new Message { Role = "user", Content = message };
                _conversation.Messages.Add(userMessage);

                var response = await _httpClient.PostAsJsonAsync("http://95.99.2.118:5001/chat/", _conversation.Messages);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                var chatbotMessage = new Message { Role = "assistant", Content = responseBody };
                userMessage.ConversationID = _conversation.ID;
                chatbotMessage.ConversationID = _conversation.ID;

                _conversation.Messages.Add(chatbotMessage);
                _conversation.LastModified = DateTime.Now;

                if (string.IsNullOrEmpty(_conversation.Title))
                {
                    _conversation.Title = await GetConversationTitle(_conversation.Messages);
                }

                if (convoId != null) 
                {
                    var entity = _context.Conversations.Update(_conversation).Entity;
                    await _context.SaveChangesAsync();

                    return entity;
                }

                var newEntity = await SaveConversationAsync();

                return newEntity;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Conversation> SaveConversationAsync()
        {
            var entity =_context.Conversations.Add(_conversation).Entity;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Conversation> GetConversationDetails(int id)
        {
            return await _context.Conversations.Include(c => c.Messages)
                                               .FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<List<Conversation>> GetAllConversationsAsync()
        {
            return await _context.Conversations.ToListAsync();
        }

        public async Task<string> GetConversationTitle(List<Message> conversationMessages)
        {
            var response = await _httpClient.PostAsJsonAsync("http://95.99.2.118:5001/title/", conversationMessages);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
