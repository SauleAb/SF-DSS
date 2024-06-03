namespace SF_DSS.Data.Entities
{
    public class Conversation
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime LastModified { get; set; }
        public List<Message> Messages { get; set; } 
    }
}
