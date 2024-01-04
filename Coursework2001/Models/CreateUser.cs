using System.ComponentModel.DataAnnotations;

namespace Coursework2001.Models
{
    public class CreateUser
    {
        [Required]
        [EmailAddress]
        [StringLength(320, ErrorMessage = "Email too long")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Username must be between 8 and 100 characters", MinimumLength = 8)]
        public string Username { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Password must be between 8 and 30 characters", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Marketing Language must be between 3 and 30 characters", MinimumLength = 3)]
        public string Marketing_Language { get; set; }
    }
}
