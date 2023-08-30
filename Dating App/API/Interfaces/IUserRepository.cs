using DatingApplication.DTOs;
using DatingApplication.Entities;
using DatingApplication.Helper;

namespace DatingApplication.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<PagedList<MemberDto>> GetUsersAsync(UserParam userParam);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string userName);
        Task<bool> SaveAllAsync();
    }
}
