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

        public async Task<ResponseDTO?> CreateStripeSession(StripeRequestDTO stripeRequestDTO)
        {
            return await _baseService.sendAsync(
              new()
              {
                  ApiType = StaticDetails.ApiType.POST,
                  Url = StaticDetails.OrderAPIUrl + "/api/order/CreateStripeSession",
                  Data = stripeRequestDTO
              });
        }

        public async Task<ResponseDTO?> GetAllOrders(string? userId)
        {
            return await _baseService.sendAsync(
    new()
    {
        ApiType = StaticDetails.ApiType.GET,
        Url = StaticDetails.OrderAPIUrl + "/api/order/GetOrders/" + userId,

    });

        }

        public async Task<ResponseDTO?> GetOrder(int orderHeaderId)
        {
            return await _baseService.sendAsync(
                new()
                {
                    ApiType = StaticDetails.ApiType.GET,
                    Url = StaticDetails.OrderAPIUrl + "/api/order/GetOrder/" + orderHeaderId,

                });
        }

        public async Task<ResponseDTO?> UpdateOrderStatus(int orderHeaderId, string newStatus)
        {
            return await _baseService.sendAsync(
                new()
                {
                    ApiType = StaticDetails.ApiType.POST,
                    Url = StaticDetails.OrderAPIUrl + "/api/order/UpdateOrderStatus/" + orderHeaderId,
                    Data = newStatus
                });
        }

        public async Task<ResponseDTO?> ValidateStripeSession(int orderHeaderId)
        {
            return await _baseService.sendAsync(
               new()
               {
                   ApiType = StaticDetails.ApiType.POST,
                   Url = StaticDetails.OrderAPIUrl + "/api/order/ValidateStripeSession",
                   Data = orderHeaderId
               });
        }
    }
}
