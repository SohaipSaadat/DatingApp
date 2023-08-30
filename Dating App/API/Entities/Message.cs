namespace DatingApplication.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUserName { get; set; }
        public AppUser Sender { get; set; }
        public int ReciverId { get; set; }
        public string ReciverUserName { get; set; }
        public AppUser Reciver { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime DateSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool ReciverDelete { get; set; }
    }
}
