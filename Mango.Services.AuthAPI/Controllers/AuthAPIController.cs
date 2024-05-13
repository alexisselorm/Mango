using Mango.Services.AuthAPI.Data.DTO;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDTO _response;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new();

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO dto)
        {
            var response = await _authService.Register(dto);
            if (!string.IsNullOrEmpty(response))
            {
                _response.IsSuccess = false;
                _response.Message = response;
                return BadRequest(_response);
            }

            return Ok(_response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO requestDTO)
        {
            var loginResponse = await _authService.Login(requestDTO);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);

            }
            _response.Result = loginResponse;
            return Ok(_response);
        }
    }
}
