
using Mango.Services.RewardsAPI.Messages;

namespace Mango.Services.RewardsAPI.Service.IService
{
    public interface IRewardService
    {
        Task<bool> UpdateRewards(RewardMessage rewardsMessage);

    }
}
