using DatingApplication.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "RequiredAdminRole")]
        [HttpGet("UsersWithRoles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                        .OrderBy(u => u.UserName)
                        .Select(u => new
                        {
                            u.Id,
                            UserName = u.UserName,
                            Roles = u.UserRole.Select(r => r.Role.Name).ToList(),
                        }).ToListAsync();
            return Ok(users);
        }

        [Authorize(Policy = "RequiredAdminRole")]
        [HttpPost("EditRole/{userName}")]
        public async Task<ActionResult> EditRole(string userName, [FromQuery]string role)
        {
            if (string.IsNullOrEmpty(role)) return BadRequest("You must select at least one role");
            var selectedRoles = role.Split(',').ToList();
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add role");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded) return BadRequest("Failed to remove role");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratorPhotoRole")]
        [HttpGet("PhotosForModerator")]
        public ActionResult GetPhotoForModeration() 
        {
            return Ok("Admins and moderator can see this");
        }
    }
}
