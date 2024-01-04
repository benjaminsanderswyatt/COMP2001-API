﻿using Azure;
using Coursework2001.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        //GET user
        [HttpGet("get")]
        public async Task<ActionResult<string>> GetUser(string email)
        {
            //TODO: this is a test validate
            string authenticationResult = await Authenticate.AuthenticateUser("testEmail", "testPassword");




            try
            {
                var userWithActivities = await _context.Users
                    .Where(u => u.Email == email)
                    .Include(u => u.UserActivities)
                    .ThenInclude(ua => ua.Activities)
                    .Select(u => new
                    {
                        u.Email,
                        u.Username,
                        u.About_Me,
                        u.Location,
                        u.Birthday,
                        u.Height_cm,
                        u.Weight_kg,
                        u.Pref_Units_Is_Metric,
                        u.Activ_Time_Pref_Is_Speed,
                        u.Marketing_Language,
                        u.Last_Updated,
                        UserActivities = u.UserActivities.Select(ua => ua.Activities.Activity_Name).ToList()
                    })
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

        //Create a user
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] CreateUser createUser)
        {
            try
            {
                // Check if valid
                if (!ModelState.IsValid)
                {
                    //Return what is wrong with the data validation
                    return BadRequest(ModelState);
                }

                using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("CW2.CreateProfile", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Username", createUser.Username));
                        cmd.Parameters.Add(new SqlParameter("@Email", createUser.Email));
                        cmd.Parameters.Add(new SqlParameter("@Password", createUser.Password));
                        cmd.Parameters.Add(new SqlParameter("@Marketing_Language", createUser.Marketing_Language));

                        //Execute the stored procedure
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

        // Update user details
        [HttpPut("update")]
        public async Task<ActionResult<string>> UpdateUser(string email, [FromBody] UpdateUser updateUser)
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


        //Delete a user
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