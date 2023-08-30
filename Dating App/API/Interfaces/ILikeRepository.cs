using DatingApplication.DTOs;
using DatingApplication.Entities;
using DatingApplication.Helper;

namespace DatingApplication.Interfaces
{
    public interface ILikeRepository
    {
        Task<LikeUser> GetUserLike(int sourceId, int targetId);
        Task<AppUser> GetUserWithLikes(int UserId);
        Task<PagedList<LikeDto>>  GetUsersLike(LikesParam likesParams);
    }
}
