using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Logout()
        {
            //RegistrationRequestDTO dto = new();
            return View();
        }
    }
}
