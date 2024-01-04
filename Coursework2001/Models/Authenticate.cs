using System.Text;

namespace Coursework2001.Models
{
    public class Authenticate
    {
        public static async Task<string> AuthenticateUser(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                //admin validate uri
                string apiUrl = "https://web.socem.plymouth.ac.uk/COMP2001/auth/api/users";

                //body json
                string jsonBody = $"{{\"Email\":\"{email}\", \"Password\":\"{password}\"}}";
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                //Check the response status
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    //TODO: I will need to check if the request content returned valid or invalid
                    //this will then create a session token and mark the session as being logged in by the user


                    return "Request successful. Response: " + result;

                }
                else
                {
                    return "An error occurred: " + response;
                }
            }
        }
    }
}