namespace SimpleChatApp.DataAccess.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
