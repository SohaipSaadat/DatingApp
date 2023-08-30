using DatingApplication.DTOs;
using DatingApplication.Entities;
using DatingApplication.Helper;
using DatingApplication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApplication.Data
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _context;

        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<LikeUser> GetUserLike(int sourceId, int targetId)
        {
            return await _context.Likes.FindAsync(sourceId, targetId);
        }

        public async Task<PagedList<LikeDto>> GetUsersLike(LikesParam likesParam)
        {
            var likesusers = _context.Likes.AsQueryable();
            var users = _context.Users.AsQueryable();

            if(likesParam.Predicate == "Liked")
            {
                likesusers = likesusers.Where(like=> like.SourceUSerId == likesParam.UserId);
                users = likesusers.Select(like => like.TargetUser);
            }

            if (likesParam.Predicate == "LikedBy")
            {
                likesusers = likesusers.Where(like => like.TargetUserId == likesParam.UserId);
                users = likesusers.Select(like => like.SourceUser);
            }

            var likesDto = users.Select(user => new LikeDto 
            {
                Id = user.Id,
                Age = 20,
                City = user.City,
                KnownAs = user.KnownAs,
                PhotoUrl = user.Photos.FirstOrDefault(p=> p.IsMAin).Url,
                UserName = user.UserName
            });
            
            return await PagedList<LikeDto>.CreateAsync(likesDto, likesParam.PageNumber, likesParam.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int UserId)
        {
            return await _context.Users
                        .Include(l => l.LikedUsers)
                        .FirstOrDefaultAsync(l => l.Id == UserId);
        }
    }
}
