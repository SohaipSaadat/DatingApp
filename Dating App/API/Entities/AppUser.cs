using Microsoft.AspNetCore.Identity;

namespace DatingApplication.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string? KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; } = null!;
        public string Introduction { get; set; } = null!;
        public string LookingFor { get; set; } = null!;
        public string Interests { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public List<LikeUser>? LikedUsers { get; set; }
        public List<LikeUser>? LikedByUsers { get; set; }


        public List<Message>  MessagesSent { get; set; }
        public List<Message> MessagesRecived { get; set; }

        public ICollection<AppUserRole> UserRole { get; set; }

    }
}
