
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {

            _baseService = baseService;

        }
        public async Task<ResponseDTO?> AssignToRole(RegistrationRequestDTO dto)
        {
            return await _baseService.sendAsync(
           new()
           {
               ApiType = StaticDetails.ApiType.POST,
               Data = dto,
               Url = StaticDetails.AuthAPIUrl + "/api/auth/AssignToRole"
           });
        }

        public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO dto)
        {
            return await _baseService.sendAsync(
     new()
     {
         ApiType = StaticDetails.ApiType.POST,
         Data = dto,
         Url = StaticDetails.AuthAPIUrl + "/api/auth/login"
     }, withBearer: false);
        }

        public async Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO dto)
        {
            return await _baseService.sendAsync(
     new()
     {
         ApiType = StaticDetails.ApiType.POST,
         Data = dto,
         Url = StaticDetails.AuthAPIUrl + "/api/auth/register"
     }, withBearer: false);
        }
    }
}
