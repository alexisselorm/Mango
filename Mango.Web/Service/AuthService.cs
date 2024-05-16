using Mango.Web.Data;
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
        public async Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO dto)
        {
            return await _baseService.sendAsync(
           new()
           {
               ApiType = StaticDetails.ApiType.POST,
               Data = dto,
               Url = StaticDetails.CouponAPIUrl + "/api/auth/AssignRole"
           });
        }

        public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO dto)
        {
            return await _baseService.sendAsync(
     new()
     {
         ApiType = StaticDetails.ApiType.POST,
         Data = dto,
         Url = StaticDetails.CouponAPIUrl + "/api/auth/login"
     });
        }

        public async Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO dto)
        {
            return await _baseService.sendAsync(
     new()
     {
         ApiType = StaticDetails.ApiType.POST,
         Data = dto,
         Url = StaticDetails.CouponAPIUrl + "/api/auth/register"
     });
        }
    }
}
