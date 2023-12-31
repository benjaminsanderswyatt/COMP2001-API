using Coursework2001.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // GET: /users
        [HttpGet("{userId}")]
        public async Task<ActionResult<String>> GetUser(int userId)
        {


            return "test" + userId;
        }




    }
}
