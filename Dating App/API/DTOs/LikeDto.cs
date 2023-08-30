namespace DatingApplication.DTOs
{
    public class LikeDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public int Age { get; set; }
        public string KnownAs { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PhotoUrl { get; set; } = null!;
    }
}
