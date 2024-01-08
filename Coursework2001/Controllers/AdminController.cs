using Coursework2001.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Coursework2001.Controllers
{
    /// <summary>
    /// Controller for managing admin actions
    /// </summary>
    [Route("api/admin/")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly COMP2001_BSanderswyattContext _context;

        public AdminController(COMP2001_BSanderswyattContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Archives a user
        /// </summary>
        /// <param name="email">Email of the user to be archived</param>
        /// <returns>ActionResult saying how the archive went</returns>
        [HttpPost("archive")]
        [Authorize]
        public async Task<ActionResult<string>> ArchiveUser(ArchiveUser archiveUser)
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
                            checkUserCmd.Parameters.Add(new SqlParameter("@Email", archiveUser.Email));

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

                                cmd.Parameters.Add(new SqlParameter("@Email", archiveUser.Email));

                                // Execute the stored procedure (ArchiveUser)
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

        /// <summary>
        /// Unarchives a user
        /// </summary>
        /// <param name="email">Email of the user to be unarchived</param>
        /// <returns>ActionResult saying how the unarchive went</returns>
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

                                // Execute the stored procedure (UnArchiveUser)
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