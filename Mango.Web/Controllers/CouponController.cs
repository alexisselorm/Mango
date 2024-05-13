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
            List<CouponDTO?> list = [];
            ResponseDTO response = await _couponService.GetAllCoupons();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDTO dto)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await _couponService.CreateCouponAsync(dto);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }

            }
            return View();
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDTO response = await _couponService.GetCouponById(couponId);

            if (response != null && response.IsSuccess)
            {
                CouponDTO? model = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDTO coupon)
        {
            ResponseDTO response = await _couponService.DeleteCouponAsync(coupon.CouponId);
            _logger.LogInformation("here");
            if (response != null && response.IsSuccess)
            {
                CouponDTO? model = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
                return RedirectToAction(nameof(CouponIndex));
            }
            return View(coupon);
        }
    }
}
