﻿namespace Mango.Services.OrderAPI.Models.DTO
{
    public class StripeRequestDTO
    {
        public string? StripeSessionId { get; set; }
        public string? StripeSessionUrl { get; set; }
        public string ApproveUrl { get; set; }
        public string CancelUrl { get; set; }
        public OrderHeaderDTO OrderHeader { get; set; }
    }
}
