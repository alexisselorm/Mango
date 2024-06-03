using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {

            _baseService = baseService;

        }

        public async Task<ResponseDTO?> ApplyCouponAsync(CartDTO dto)
        {
            return await _baseService.sendAsync(
                new()
                {
                    ApiType = StaticDetails.ApiType.POST,
                    Data = dto,
                    Url = StaticDetails.ShoppingCartAPIUrl + "/api/cart/ApplyCoupon"
                });
        }

        public async Task<ResponseDTO?> EmailCart(CartDTO dto)
        {
            return await _baseService.sendAsync(
                new()
                {
                    ApiType = StaticDetails.ApiType.POST,
                    Data = dto,
                    Url = StaticDetails.ShoppingCartAPIUrl + "/api/cart/EmailCartRequest"
                });
        }

        public async Task<ResponseDTO?> GetCartByUserIdAsync(string userId)
        {
            return await _baseService.sendAsync(
new()
{
    ApiType = StaticDetails.ApiType.GET,
    Url = StaticDetails.ShoppingCartAPIUrl + "/api/cart/GetCart/" + userId
});
        }

        public async Task<ResponseDTO?> RemoveFromCartAsync(int cartDetailsid)
        {
            return await _baseService.sendAsync(
      new()
      {
          ApiType = StaticDetails.ApiType.POST,
          Url = StaticDetails.ShoppingCartAPIUrl + "/api/Cart/RemoveCart",
          Data = cartDetailsid
      });
        }
        public async Task<ResponseDTO?> UpsertCartAsync(CartDTO dto)
        {
            return await _baseService.sendAsync(
      new()
      {
          ApiType = StaticDetails.ApiType.POST,
          Url = StaticDetails.ShoppingCartAPIUrl + "/api/Cart/CartUpsert",
          Data = dto
      });
        }
    }
}
