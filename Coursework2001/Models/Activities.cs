namespace Coursework2001.Models
{
    public class Activities
    {
        public int ActivityID { get; set; }
        public string Activity_Name { get; set; }

        // Navigation property for User-Activities relationship
        public List<UserActivities> UserActivities { get; set; }
    }
}
