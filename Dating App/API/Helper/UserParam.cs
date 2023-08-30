namespace DatingApplication.Helper
{
    public class UserParam : PaginationParams
    {
        public string CurrentUserNmae { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string OrderBy { get; set; } = "LastActive";

    }
}
