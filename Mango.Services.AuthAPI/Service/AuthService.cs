using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Data.DTO;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthService(RoleManager<IdentityRole> roleManager, AppDbContext db, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _db = db;
            _roleManager = roleManager;

        }
        public Task<LoginResponeDTO> Login(LoginRequestDTO requestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> Register(RegistrationRequestDTO requestDTO)
        {
            AppUser user = new()
            {
                UserName = requestDTO.Email,
                Email = requestDTO.Email,
                NormalizedEmail = requestDTO.Email.ToUpper(),
                Name = requestDTO.Name,
                PhoneNumber = requestDTO.PhoneNumber,
            };
            try
            {
                var result = _userManager.CreateAsync(user);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
