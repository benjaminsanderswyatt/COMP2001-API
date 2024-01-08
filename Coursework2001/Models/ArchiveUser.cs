using System.ComponentModel.DataAnnotations;

namespace Coursework2001.Models
{
    public class ArchiveUser
    {
        [Required]
        [EmailAddress]
        [StringLength(320, ErrorMessage = "Email too long")]
        public string Email { get; set; }

    }
}
