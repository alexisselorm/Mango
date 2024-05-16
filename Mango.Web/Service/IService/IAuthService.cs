using Mango.Web.Data;
using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        public Task<ResponseDTO?> LoginAsync(LoginRequestDTO dto);
        public Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO dto);
        public Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO dto);
    }
}
