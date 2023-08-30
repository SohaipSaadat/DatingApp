using Microsoft.AspNetCore.Identity;

namespace DatingApplication.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRole { get; set; }

    }
}
