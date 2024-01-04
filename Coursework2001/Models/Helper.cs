using System.Data;


namespace Coursework2001.Models
{
    public class Helper
    {
        //what data should be returned in get request depending on the users auth
        public static object UserDataToShow(User user, bool isSelfOrAdmin)
        {
            if (isSelfOrAdmin)
            {
                //Self or admin can see all data
                var userData = new
                {
                    user.Email,
                    user.Username,
                    user.About_Me,
                    user.Location,
                    user.Birthday,
                    user.Height_cm,
                    user.Weight_kg,
                    user.Pref_Units_Is_Metric,
                    user.Activ_Time_Pref_Is_Speed,
                    user.Marketing_Language,
                    user.Last_Updated,
                    UserActivities = user.UserActivities.Select(ua => ua.Activities.Activity_Name).ToList()
                };
                return userData;
            }
            else
            {
                //return publicly accessible data
                var userData = new
                {
                    user.Email,
                    user.Username,
                    user.About_Me,
                    user.Location,
                    user.Birthday,
                    UserActivities = user.UserActivities.Select(ua => ua.Activities.Activity_Name).ToList()
                };
                return userData;
            }
        }


    }
}
    

