using SF_DSS.Data.Entities;

namespace SF_DSS.Models
{
    public class ChatbotModel
    {
        public Conversation? Conversation { get; set; }
        public List<Conversation> Conversations { get; set; } = new List<Conversation>();
    }
}
