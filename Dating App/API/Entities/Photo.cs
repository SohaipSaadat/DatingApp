namespace DatingApplication.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public bool IsMAin { get; set; }
        public string PublicId { get; set; } = null!;
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}