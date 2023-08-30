using DatingApplication.Entities;

namespace DatingApplication.TokenServices
{
    public interface ITokenService
    {
        Task<string> createToken(AppUser appUser);
    }
}
