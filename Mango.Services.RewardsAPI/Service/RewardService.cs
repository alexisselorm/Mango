using Mango.Services.RewardsAPI.Data;
using Mango.Services.RewardsAPI.Messages;
using Mango.Services.RewardsAPI.Models;
using Mango.Services.RewardsAPI.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.EmailAPI.Service
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public RewardService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task<bool> UpdateRewards(RewardMessage rewardsMessage)
        {
            try
            {
                Rewards rewards = new()
                {
                    OrderId = rewardsMessage.OrderId,
                    RewardActivity = rewardsMessage.RewardsActivity,
                    UserId = rewardsMessage.UserId,
                    RewardDate = DateTime.Now
                };
                await using var _db = new AppDbContext(_dbOptions);
                await _db.Rewards.AddAsync(rewards);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


    }
}
