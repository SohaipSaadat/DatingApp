namespace DatingApplication.Entities
{
    public class LikeUser
    {
        public int SourceUSerId { get; set; }
        public AppUser SourceUser { get; set; }

        public int TargetUserId { get; set; }
        public AppUser TargetUser { get; set; }

    }
}
