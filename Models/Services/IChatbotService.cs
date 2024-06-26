﻿using SF_DSS.Data.Entities;

namespace SF_DSS.Models.Services
{
    public interface IChatbotService
    {
        Task<Conversation> GetResponse(string message, int? convoId);
        Task<Conversation> SaveConversationAsync();
        Task<Conversation> GetConversationDetails(int id);
        Task<List<Conversation>> GetAllConversationsAsync();
        Task<string> GetConversationTitle(List<Message> conversationMessages);
    }
}
