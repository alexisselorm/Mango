using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {

            _baseService = baseService;

        }
        public async Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDTO)
        {
            return await _baseService.sendAsync(
           new()
           {
               ApiType = StaticDetails.ApiType.POST,
               Url = StaticDetails.CouponAPIUrl + "/api/coupon",
               Data = couponDTO
           });
        }

        public async Task<ResponseDTO?> DeleteCouponAsync(int id)
        {
            return await _baseService.sendAsync(
           new()
           {
               ApiType = StaticDetails.ApiType.DELETE,
               Url = StaticDetails.CouponAPIUrl + "/api/coupon/" + id
           });
        }

        public async Task<ResponseDTO?> GetAllCoupons()
        {
            return await _baseService.sendAsync(
                new()
                {
                    ApiType = StaticDetails.ApiType.GET,
                    Url = StaticDetails.CouponAPIUrl + "/api/coupon"
                });
        }

        public async Task<ResponseDTO?> GetCouponAsync(string code)
        {
            return await _baseService.sendAsync(
           new()
           {
               ApiType = StaticDetails.ApiType.GET,
               Url = StaticDetails.CouponAPIUrl + "/api/coupon/" + code
           });
        }

        public async Task<ResponseDTO?> GetCouponById(int id)
        {
            return await _baseService.sendAsync(
           new()
           {
               ApiType = StaticDetails.ApiType.GET,
               Url = StaticDetails.CouponAPIUrl + "/api/coupon/" + id
           });
        }

        public async Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDTO)
        {
            return await _baseService.sendAsync(
       new()
       {
           ApiType = StaticDetails.ApiType.PUT,
           Url = StaticDetails.CouponAPIUrl + "/api/coupon/",
           Data = couponDTO
       });
        }
    }
}
