using Mango.Services.AuthAPI.Data.DTO;

namespace Mango.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<UserDTO> Register(RegistrationRequestDTO requestDTO);
        Task<LoginResponeDTO> Login(LoginRequestDTO requestDTO);
    }
}
