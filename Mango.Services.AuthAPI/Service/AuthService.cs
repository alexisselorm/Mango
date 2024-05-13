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

        public async Task<UserDTO> Register(RegistrationRequestDTO requestDTO)
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
                var result = await _userManager.CreateAsync(user, requestDTO.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.AppUsers.First(u => user.UserName == requestDTO.Email);
                    UserDTO userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return userDto;
                }
            }
            catch (Exception ex)
            {
                return new UserDTO();
            }
        }
    }
}
