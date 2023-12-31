using Azure;
using Coursework2001.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Coursework2001.Controllers
{
    [Route("api/user/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly COMP2001_BSanderswyattContext _context;

        public UsersController(COMP2001_BSanderswyattContext context)
        {
            _context = context;
        }


        //Create a user
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] CreateUser createUser)
        {
            // Retrieve the connection string from appsettings.json
            try
            {
                // Check if email is valid
                try
                {
                    var mailAddress = new System.Net.Mail.MailAddress(createUser.Email);
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid Email");
                }

                using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("CW2.CreateProfile", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        cmd.Parameters.Add(new SqlParameter("@Username", createUser.Username));
                        cmd.Parameters.Add(new SqlParameter("@Email", createUser.Email));
                        cmd.Parameters.Add(new SqlParameter("@Password", createUser.Password));
                        cmd.Parameters.Add(new SqlParameter("@Marketing_Language", createUser.Marketing_Language));

                        // Execute the stored procedure
                        await cmd.ExecuteNonQueryAsync();

                    }
                }
                return Ok("Registration successful.");
            } catch (SqlException ex){

                //does email already exist (not unique)
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    return BadRequest("Email already exists. Could not register user.");
                }
                else
                {
                    return StatusCode(500, "An error occurred: " + ex.Message);
                }
            } catch (Exception ex){

                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }




        // DELETE: user/{Email}
        [HttpDelete("delete")]
        public async Task<ActionResult<string>> DeleteUser(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("CW2.DeleteUser", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameter for email
                        cmd.Parameters.Add(new SqlParameter("@Email", email));

                        // Execute the stored procedure
                        int result = await cmd.ExecuteNonQueryAsync();

                        // Was the delete successful
                        if (result == 1)
                        {
                            //success
                            return Ok("User deleted successfully.");
                        }
                        else
                        {
                            // User not found
                            return NotFound("User not found.");
                        }
                    }
                }

                
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }


    }
}