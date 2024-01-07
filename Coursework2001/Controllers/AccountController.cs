using Coursework2001.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Coursework2001.Controllers
{
    [Route("api/account/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly COMP2001_BSanderswyattContext _context;

        public AccountController(COMP2001_BSanderswyattContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> ValidUser(Login login)
        {
            AuthResult authResult = await Authenticate.AuthenticateUserOrAdmin(login, _context);

            if (authResult.IsArchived)
            {
                return Unauthorized(new { userArchived = true });
            }

            if (authResult.IsValid)
            {
                string authToken = Authenticate.GenerateAuthToken(login.Email, authResult.IsAdmin);

                this.Response.Headers.Append("api_key", authToken);

                HttpContext.Session.SetString(authToken, "verified");

                return Ok(new { verified = true, token = authToken });
            }

            // Unauthorized
            return Unauthorized(new { verified = false });
        }

        //CREATE a user
        [HttpPost("register")]
        [EndpointDescription("Registers a new user.")]
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
            } catch (SqlException ex) {

                //does email already exist (not unique)
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    return BadRequest("Email already exists. Could not register user.");
                }
                else
                {
                    return StatusCode(500, "An error occurred: " + ex.Message);
                }
            } catch (Exception ex) {

                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }


    }

}