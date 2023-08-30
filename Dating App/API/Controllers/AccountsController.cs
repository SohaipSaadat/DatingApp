using AutoMapper;
using DatingApplication.Data;
using DatingApplication.DTOs;
using DatingApplication.Entities;
using DatingApplication.TokenServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDot>> Register(RegisterDto register)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == register.UserName)) return BadRequest("This Name is already exist!");

            var user = _mapper.Map<AppUser>(register);

           
            user.UserName = register.UserName.ToLower();
           
                
            var result = await _userManager.CreateAsync(user, register.Password);


            if (!result.Succeeded) return BadRequest(result.Errors.ToString());

            var roleResult =  await _userManager.AddToRoleAsync(user, "Member");

            if(!roleResult.Succeeded) return BadRequest(roleResult.Errors.ToString());
            return new UserDot()
            {
                UserName = user.UserName,
                Token = await _tokenService.createToken(user),
                Gender = user.Gender,
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDot>> Login(LoginDto login)
        {
            var user = await _userManager.Users.Include(p=> p.Photos).FirstOrDefaultAsync(u=> u.UserName == login.UserName);

            if (user == null) return Unauthorized("Invalid username or password");

            var result = await _userManager.CheckPasswordAsync(user, login.Password);

            if(result) return Unauthorized("Invalid username or password");

            return new UserDot()
            {
                UserName = user.UserName,
                Token = await _tokenService.createToken(user),
                Gender = user.Gender,
                PhotoUrl = user.Photos.FirstOrDefault(p=> p.IsMAin)?.Url
            };

        }
    }
}
