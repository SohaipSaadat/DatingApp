namespace DatingApplication.Helper
{
    public class LikesParam : PaginationParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; } = "Liked";
    }
}
