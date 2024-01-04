using System.ComponentModel.DataAnnotations;

namespace Coursework2001.Models
{
    public class UpdateUser
    {
        [Required]
        [StringLength(100, ErrorMessage = "Username must be between 8 and 100 characters", MinimumLength = 8)]
        public string Username { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Password must be between 8 and 30 characters", MinimumLength = 8)]
        public string Password { get; set; }

        [StringLength(500, ErrorMessage = "About Me is too long")]
        public string About_Me { get; set; }

        [StringLength(160, ErrorMessage = "Location is too long")]
        public string Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Range(0.00, 999.99, ErrorMessage = "Height must be between 0.00 and 999.99")]
        public decimal? Height_cm { get; set; }

        [Range(0.00, 999.99, ErrorMessage = "Weight must be between 0.00 and 999.99")]
        public decimal? Weight_kg { get; set; }

        [Required]
        public bool Pref_Units_Is_Metric { get; set; }

        [Required]
        public bool Activ_Time_Pref_Is_Speed { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Marketing Language must be between 3 and 30 characters", MinimumLength = 3)]
        public string Marketing_Language { get; set; }

        // List of activities
        public List<int> Activities { get; set; }
    }
}
