﻿namespace Mango.Services.RewardsAPI.Messages
{
    public class RewardMessage
    {
        public int OrderId { get; set; }
        public int RewardsActivity { get; set; }
        public string UserId { get; set; }
    }
}
