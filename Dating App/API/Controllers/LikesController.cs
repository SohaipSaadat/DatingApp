using AutoMapper;
using DatingApplication.DTOs;
using DatingApplication.Entities;
using DatingApplication.Extentions;
using DatingApplication.Helper;
using DatingApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [TypeFilter(typeof(LogUserActivity))]
    public class LikesController : Controller
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserRepository _userRepository;

        public LikesController(ILikeRepository likeRepository, IUserRepository userRepository)
        {
            _likeRepository = likeRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{userName}")]
        public async Task<ActionResult> AddLike(string userName)
        {
            var sourceUserId = User.GetUserId();
            var targetUser = await _userRepository.GetUserByUserNameAsync(userName);
            var sourceUser = await _likeRepository.GetUserWithLikes(sourceUserId);

            if (targetUser is null) return NotFound();
            if (sourceUser.UserName == userName) return BadRequest("You can't like yourself");
            var userLike = await _likeRepository.GetUserLike(sourceUserId, targetUser.Id);
            if (userLike is not null) return BadRequest("You alredy like this user");
            userLike = new LikeUser() { SourceUSerId =  sourceUserId , TargetUserId = targetUser.Id};
            sourceUser.LikedUsers?.Add(userLike);
            if(await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParam likesParams)
        {
             likesParams.UserId = User.GetUserId();
            var likedUser = await _likeRepository.GetUsersLike(likesParams);
            Response.AddPaginationHeader(new PaginationHeader(likedUser.CurrentPage, likedUser.PageSize,likedUser.TotalPages, likedUser.TotalCount));
            return Ok(likedUser);
        }
    }
}
