﻿using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDTO?> CreateOrderAsync(CartDTO cartDTO);
        Task<ResponseDTO?> CreateStripeSession(StripeRequestDTO stripeRequestDTO);
        Task<ResponseDTO?> ValidateStripeSession(int orderHeaderId);
        Task<ResponseDTO?> GetAllOrders(string? userId);
        Task<ResponseDTO?> GetOrder(int orderHeaderId);
        Task<ResponseDTO?> UpdateOrderStatus(int orderHeaderId, string newStatus);

    }
}
