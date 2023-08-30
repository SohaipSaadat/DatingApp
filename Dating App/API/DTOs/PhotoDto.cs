namespace DatingApplication.DTOs
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public bool IsMAin { get; set; }
        public string PublicId { get; set; } = null!;
    }
}