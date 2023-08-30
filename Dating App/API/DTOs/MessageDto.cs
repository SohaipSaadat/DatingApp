using DatingApplication.Entities;

namespace DatingApplication.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUserName { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int ReciverId { get; set; }
        public string ReciverUserName { get; set; }
        public string ReciverPhotoUrl { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime DateSent { get; set; } = DateTime.UtcNow;
    }
}
