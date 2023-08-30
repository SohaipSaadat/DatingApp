using DatingApplication.Data;
using DatingApplication.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ErrorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "Secret Text";
        }

        [HttpGet("notFound")]
        public ActionResult<AppUser> GetNotFound()
        {
            var user = _context.Users.Find(-1);
            return user is null ? NotFound() : user ;
        }


        [HttpGet("serverError")]
        public ActionResult<string> GetServerError()
        {
            var user = _context.Users.Find(-1);
            var userToString = user.ToString();
            return userToString;
        }

        [HttpGet("BadRequest")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This is not a good request");
        }
    }
}
