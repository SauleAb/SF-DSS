namespace SF_DSS.Models.Services
{
    public interface IChatbotService
    {
        Task<string> GetResponse(string message);
        Task SaveConversationAsync();
    }
}
