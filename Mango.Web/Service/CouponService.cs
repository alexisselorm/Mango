using Mango.Web.Models;
using Mango.Web.Service.IService;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {

            _baseService = baseService;

        }
        public Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> DeleteCouponAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> GetAllCoupons()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> GetCouponAsync(string code)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> GetCouponById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDTO)
        {
            throw new NotImplementedException();
        }
    }
}
