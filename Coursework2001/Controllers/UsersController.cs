using Coursework2001.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

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
        
        //GET self data
        [HttpGet("get-self")]
        [Authorize]
        public async Task<ActionResult<string>> GetSelf()
        {
            //Get the users email token claims
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email != null)
            {
                return await GetAllUserData(email, true);
            }

            return BadRequest("Please login first");
        }

        //GET someones data
        [HttpGet("get-user")]
        [Authorize]
        public async Task<ActionResult<string>> GetUser(string email)
        {
            //Get the users role from token claims
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "admin")
            {
                return await GetAllUserData(email, true);
            }
            else
            {
                return await GetAllUserData(email, false);
            }
        }
        //gets the user data 
        private async Task<ActionResult<string>> GetAllUserData(string email, bool isSelfOrAdmin)
        {
            try
            {
                var userWithActivities = await _context.Users
                .Where(u => u.Email == email)
                .Include(u => u.UserActivities)
                .ThenInclude(ua => ua.Activities)
                .Select(u => GetHelper.UserDataToShow(u, isSelfOrAdmin))
                .FirstOrDefaultAsync();
            
                if (userWithActivities == null)
                {
                    return NotFound("User not found");
                }

                return Ok(userWithActivities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        //UPDATE user details
        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult<string>> UpdateUser([FromBody] UpdateUser updateUser)
        {
            //Get the users email token claims
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email != null)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        //Return what is wrong with the inputted data
                        return BadRequest(ModelState);
                    }

                    // Check if email is valid
                    try
                    {
                        var mailAddress = new System.Net.Mail.MailAddress(email);
                    }
                    catch (FormatException)
                    {
                        return BadRequest("Invalid Email");
                    }

                    using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                    {
                        await connection.OpenAsync();

                        bool userFound = false;

                        //check if the user with email exists
                        var userExistsQuery = "SELECT 1 FROM CW2.Users WHERE Email = @Email";
                        using (SqlCommand checkUserCmd = new SqlCommand(userExistsQuery, connection))
                        {
                            checkUserCmd.Parameters.Add(new SqlParameter("@Email", email));

                            var result = await checkUserCmd.ExecuteScalarAsync();
                            if (result != null)
                            {
                                userFound = true;
                            }
                        }

                        if (userFound)
                        {
                            //run the stored procedure
                            using (SqlCommand cmd = new SqlCommand("CW2.UpdateUserAndActivities", connection))
                            {

                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@Email", email));
                                cmd.Parameters.Add(new SqlParameter("@Username", updateUser.Username));
                                cmd.Parameters.Add(new SqlParameter("@Password", updateUser.Password));
                                cmd.Parameters.Add(new SqlParameter("@About_Me", updateUser.About_Me));
                                cmd.Parameters.Add(new SqlParameter("@Location", updateUser.Location));
                                cmd.Parameters.Add(new SqlParameter("@Birthday", updateUser.Birthday));
                                cmd.Parameters.Add(new SqlParameter("@Height_cm", updateUser.Height_cm));
                                cmd.Parameters.Add(new SqlParameter("@Weight_kg", updateUser.Weight_kg));
                                cmd.Parameters.Add(new SqlParameter("@Pref_Units_Is_Metric", updateUser.Pref_Units_Is_Metric));
                                cmd.Parameters.Add(new SqlParameter("@Activ_Time_Pref_Is_Speed", updateUser.Activ_Time_Pref_Is_Speed));
                                cmd.Parameters.Add(new SqlParameter("@Marketing_Language", updateUser.Marketing_Language));

                                //Create the datatable for activities list
                                DataTable activityTable = new DataTable();
                                activityTable.Columns.Add("ActivityID", typeof(int));

                                foreach (var activity in updateUser.Activities)
                                {
                                    activityTable.Rows.Add(activity);
                                }

                                SqlParameter tvpParam = cmd.Parameters.AddWithValue("@ActivityList", activityTable);
                                tvpParam.SqlDbType = SqlDbType.Structured;
                                tvpParam.TypeName = "CW2.TempActivityList";


                                // Execute the stored procedure
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                        else
                        {
                            // Return a NotFound response if the user with the provided email does not exist
                            return NotFound("User not found.");
                        }

                    }

                    return Ok("User updated successfully.");
                }
                catch (SqlException ex)
                {
                    // SQL error
                    return StatusCode(500, "An error occurred: " + ex.Message);
                }
                catch (Exception ex)
                {
                    // Other error
                    return StatusCode(500, "An error occurred: " + ex.Message);
                }
            }
            else
            {
                return BadRequest("Please login first");
            }

        }
        
        //DELETE self
        [HttpDelete("delete")]
        [Authorize]
        public async Task<ActionResult<string>> DeleteUser()
        {
            //Get the users email token claims
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email != null)
            {
                return await Delete(email);
            }

            return BadRequest("Please login first");
        }

        //Delete user with the email
        private async Task<ActionResult<string>> Delete(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("CW2.DeleteUser", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Email", email));

                        //Execute the stored procedure
                        int result = await cmd.ExecuteNonQueryAsync();

                        //Was the delete successful
                        if (result == 1)
                        {
                            //success
                            return Ok("User deleted successfully.");
                        }
                        else
                        {
                            //User not found
                            return NotFound("User not found.");
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                //Sql error
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
            catch (Exception ex)
            {
                //other error
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }



    }

}