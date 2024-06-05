using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;
        public OrderService(IBaseService baseService)
        {

            _baseService = baseService;

        }


        public async Task<ResponseDTO?> CreateOrderAsync(CartDTO cartDTO)
        {
            return await _baseService.sendAsync(
          new()
          {
              ApiType = StaticDetails.ApiType.POST,
              Url = StaticDetails.OrderAPIUrl + "/api/order/CreateOrder",
              Data = cartDTO
          });
        }


    }
}
