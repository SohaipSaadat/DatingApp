using System.ComponentModel.DataAnnotations;

namespace DatingApplication.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Gender { get; set; } = null!;
        [Required]
        public string KnownAs { get; set; } = null!;
        [Required]
        public string Introduction { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [Required]
        public string Interests { get; set; } = null!;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string LookingFor { get; set; } = null!;


    }
}
