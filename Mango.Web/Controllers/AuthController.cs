using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            LoginRequestDTO dto = new() { UserName = "", Password = "" };
            return View(dto);
        }

        public IActionResult Register()
        {
            //RegistrationRequestDTO dto = new();
            return View();
        }

        public IActionResult Logout()
        {
            //RegistrationRequestDTO dto = new();
            return View();
        }
    }
}
