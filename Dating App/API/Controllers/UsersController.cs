using AutoMapper;
using DatingApplication.Data;
using DatingApplication.DTOs;
using DatingApplication.Entities;
using DatingApplication.Extentions;
using DatingApplication.Helper;
using DatingApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DatingApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [TypeFilter(typeof(LogUserActivity))]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoServices _photoServices;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoServices photoServices)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoServices = photoServices;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParam userParam)
        {
            
            var currentUser = await _userRepository.GetUserByUserNameAsync(User.GetUserName());
            userParam.CurrentUserNmae = currentUser.UserName;
            if (string.IsNullOrEmpty(userParam.Gender))
            {
                userParam.Gender = currentUser.Gender == "male" ? "female" : "male";
            }
            var users = await _userRepository.GetUsersAsync(userParam);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalPages, users.TotalCount));
            return Ok(users);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id) 
        {
            var users = await _userRepository.GetUserByIdAsync(id);
            var userMapped = _mapper.Map<MemberDto>(users);
            return Ok(userMapped);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<MemberDto>> GetUser(string name)
        {
            var users = await _userRepository.GetUserByUserNameAsync(name);
            var userMapped = _mapper.Map<MemberDto>(users);
            return Ok(userMapped);
        }

        [HttpPut]
        public async Task<ActionResult> EditMember(MemberUpdateDto member)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUserNameAsync(userName!);
            if (user is null) return NotFound();
            _mapper.Map(member,user);
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());
            if (user is null) return NotFound();
            var result = await _photoServices.AddPhotoAsync(file);
            if (result.Error is not null) return BadRequest(result.Error.Message);
            var photo = new Photo()
            {
                Url = result.Url.AbsoluteUri,
                PublicId = result.PublicId,
            };

            if (user.Photos.Count == 0) photo.IsMAin = true;
            
            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetUser), new { name = user.UserName }, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());

            if (user is null) return NotFound();

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo is null) return NotFound();

            if (photo.IsMAin) return BadRequest("this is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(p => p.IsMAin);

            if (currentMain is not null) currentMain.IsMAin = false;

            photo.IsMAin = true;

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting a main phtot");
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());

            if (user is null) return NotFound();

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo is null) return NotFound();

            if (photo.IsMAin) return BadRequest("You can't delete your main photo");

            if(photo.PublicId is not null)
            {
                var result = await _photoServices.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            
            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem deleting photo");
        }
    }
}
