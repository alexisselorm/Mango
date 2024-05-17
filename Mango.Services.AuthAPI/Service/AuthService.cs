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
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(RoleManager<IdentityRole> roleManager, AppDbContext db, UserManager<AppUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.AppUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {

                if (!(await _roleManager.RoleExistsAsync(roleName)))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponeDTO> Login(LoginRequestDTO requestDTO)
        {
            var user = _db.AppUsers.FirstOrDefault(u => u.UserName.ToLower() == requestDTO.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, requestDTO.Password);
            if (user == null || isValid == false)
            {
                return new LoginResponeDTO() { User = null, Token = "" };
            }
            var roles = await _userManager.GetRolesAsync(user);
            //Generate token if user found
            var token = _jwtTokenGenerator.GenerateToken(user, roles);
            UserDTO userDTO = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };


            LoginResponeDTO loginResponeDTO = new()
            {
                User = userDTO,
                Token = token
            };
            return loginResponeDTO;
        }

        public async Task<string> Register(RegistrationRequestDTO requestDTO)
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

                    return "";
                }
                else
                {

                    return result.Errors?.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
