using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Coursework2001.Models
{
    public class Authenticate
    {
        public static async Task<AuthResult> AuthenticateUserOrAdmin(Login login, COMP2001_BSanderswyattContext _context)
        {
            //is admin?
            bool isValidAdmin = await AuthenticateAdmin(login);

            if (isValidAdmin)
            {
                return AuthResult.Success(true); //Authorised admin
            }

            //is user?
            bool isValidUser = await AuthenticateUser(login, _context);

            if (isValidUser)
            {
                return AuthResult.Success(false);//Authorised User
            }
            else
            {
                return AuthResult.Failure();
            }
        }

        public static async Task<bool> AuthenticateAdmin(Login login)
        {
            using (HttpClient client = new HttpClient())
            {
                //admin validate uri
                string apiUrl = "https://web.socem.plymouth.ac.uk/COMP2001/auth/api/users";

                //body json
                string jsonBody = $"{{\"Email\":\"{login.Email}\", \"Password\":\"{login.Password}\"}}";
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                //Check the response status
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    //result in format ["Verified","False"] or ["Verified","True"]
                    var jsonArrayResult = System.Text.Json.JsonSerializer.Deserialize<string[]>(result);

                    if (jsonArrayResult != null && jsonArrayResult[1].Equals("True", StringComparison.OrdinalIgnoreCase))
                    {
                        //Authorised admin
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public static async Task<bool> AuthenticateUser(Login login, COMP2001_BSanderswyattContext _context)
        {
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("CW2.VerifyUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Email", login.Email));
                    cmd.Parameters.Add(new SqlParameter("@Password", login.Password));

                    // Add output parameter to capture the result value
                    SqlParameter resultParameter = new SqlParameter("@Verified", SqlDbType.NVarChar, 10)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(resultParameter);

                    // Execute the stored procedure
                    await cmd.ExecuteNonQueryAsync();

                    string? result = resultParameter.Value.ToString();

                    //Was the delete successful
                    if (result != null && result.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        //success
                        return true;
                    }
                    else
                    {
                        //User not found
                        return false;
                    }
                }

            }
        }




        public static string GenerateAuthToken(string email, bool isAdmin)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, isAdmin ? "admin" : "user")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Random-Temporary-Key-For-Security-Reasons")); //Encodes for security
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "BSW-Api",
                audience: "user",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), //Token expires in 30 mins
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}