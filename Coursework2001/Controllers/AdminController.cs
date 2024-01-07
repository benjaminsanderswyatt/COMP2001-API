using Coursework2001.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Coursework2001.Controllers
{
    [Route("api/admin/")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly COMP2001_BSanderswyattContext _context;

        public AdminController(COMP2001_BSanderswyattContext context)
        {
            _context = context;
        }

        //ARCHIVE user
        [HttpPost("archive")]
        [Authorize]
        public async Task<ActionResult<string>> ArchiveUser(string email)
        {
            //Get the users role from token claims
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "admin")
            {
                try
                {
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
                            using (SqlCommand cmd = new SqlCommand("CW2.ArchiveUser", connection))
                            {

                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@Email", email));

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

                    return Ok("User has been archived.");

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

            return BadRequest("User is not an admin");

        }

        //UNARCHIVE user
        [HttpPost("unarchive")]
        [Authorize]
        public async Task<ActionResult<string>> UnArchiveUser(string email)
        {
            //Get the users role from token claims
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "admin")
            {
                try
                {
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
                            using (SqlCommand cmd = new SqlCommand("CW2.UnArchiveUser", connection))
                            {

                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@Email", email));

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

                    return Ok("User has been unarchived.");

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

            return BadRequest("User is not an admin");

        }





    }

}