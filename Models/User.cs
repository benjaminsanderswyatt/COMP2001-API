namespace Coursework2001.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string About_Me { get; set; }
        public string Location { get; set; }
        public DateTime? Birthday { get; set; }
        public decimal? Height_cm { get; set; }
        public decimal? Weight_kg { get; set; }
        public bool Pref_Units_Is_Metric { get; set; }
        public bool Activ_Time_Pref_Is_Speed { get; set; }
        public string Marketing_Language { get; set; }
        public bool Is_Archived { get; set; }
        public DateTime? Last_Updated { get; set; }

        // Navigation property for User-Activities relationship
        public List<UserActivities> UserActivities { get; set; }
    }
}
