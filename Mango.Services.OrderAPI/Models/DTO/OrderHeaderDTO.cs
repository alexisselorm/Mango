﻿namespace Mango.Services.OrderAPI.Models.DTO
{
    public class OrderHeaderDTO
    {
        public int Order { get; set; }
        public string UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double TotalAmount { get; set; }

        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public DateTime OrderTime { get; set; }

        public string? Status { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? StripeSessionId { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }

    }
}
