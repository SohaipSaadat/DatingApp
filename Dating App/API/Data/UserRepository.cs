using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApplication.DTOs;
using DatingApplication.Entities;
using DatingApplication.Helper;
using DatingApplication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApplication.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PagedList<MemberDto>> GetUsersAsync(UserParam userParam)
        {
            //var query = _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking();
            var query = _context.Users.AsQueryable();
            query = query.Where(u => u.UserName != userParam.CurrentUserNmae);
            query = query.Where(u=> u.Gender == userParam.Gender);
            query = userParam.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(
                query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(), 
                userParam.PageNumber, userParam.PageSize);
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(p=> p.Id == id);
        }
        public async Task<AppUser> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(p => p.UserName == userName);
        }


        public void Update(AppUser user) => _context.Entry(user).State = EntityState.Modified;

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
