using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        private readonly ILogger<CouponController> _logger;
        public CouponController(ICouponService couponService, ILogger<CouponController> logger)
        {
            _couponService = couponService;
            _logger = logger;
        }
        public async Task<IActionResult> CouponIndex()
        {
            _logger.LogInformation("Something is wrong");
            List<CouponDTO?> list = [];
            var response = await _couponService.GetAllCoupons();
            _logger.LogInformation($"{response.Message}");
            _logger.LogInformation($"{response.Result.ToString()}");
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}
