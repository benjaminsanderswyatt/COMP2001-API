﻿namespace Coursework2001.Models
{
    public class UserActivities
    {
        public int UserActivitiesID { get; set; }
        public string Email { get; set; }
        public int ActivityID { get; set; }

        //Navigation properties for the relationships
        public User User { get; set; }
        public Activities Activities { get; set; }
    }
}
