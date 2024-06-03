namespace SF_DSS.Data.Entities
{
    public class Message
    {
        public int ID { get; set; }
        public string Role { get; set; }
        public string Content { get; set; }
        public int ConversationID {  get; set; }
    }
}
