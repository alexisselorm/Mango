using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AppUser appUser, IEnumerable<string> roles);
    }
}
