using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;

        }

        public IActionResult Login()
        {
            LoginRequestDTO dto = new() { UserName = "", Password = "" };
            return View(dto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            //RegistrationRequestDTO dto = new();
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=StaticDetails.RoleAdmin,Value=StaticDetails.RoleAdmin},
                new SelectListItem{Text=StaticDetails.RoleCustomer,Value=StaticDetails.RoleCustomer }
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO dto)
        {
            ResponseDTO responseDTO = await _authService.RegisterAsync(dto);
            ResponseDTO assignRole;

            if (responseDTO != null && responseDTO.IsSuccess)
            {
                if (string.IsNullOrEmpty(dto.Role))
                {
                    dto.Role = StaticDetails.RoleCustomer;
                }
                assignRole = await _authService.AssignToRole(dto);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=StaticDetails.RoleAdmin,Value=StaticDetails.RoleAdmin},
                new SelectListItem{Text=StaticDetails.RoleCustomer,Value=StaticDetails.RoleCustomer }
            };
            ViewBag.RoleList = roleList;
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            ResponseDTO result = await _authService.LoginAsync(dto);
            if (result != null && result.IsSuccess)
            {
                LoginResponeDTO response = JsonConvert.DeserializeObject<LoginResponeDTO>(Convert.ToString(result.Result));
                await SignInUser(response);
                _tokenProvider.setToken(response.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", result.Message);
                return View(dto);
            }
        }

        private async Task SignInUser(LoginResponeDTO dto)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(dto.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));


            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.clearToken();
            //RegistrationRequestDTO dto = new();
            return RedirectToAction("Index", "Home");
        }
    }
}
