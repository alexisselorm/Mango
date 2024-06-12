using Mango.Services.EmailAPI.Messages;
using Mango.Services.EmailAPI.Models.DTO;

namespace Mango.Services.EmailAPI.Service.IService
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDTO cartDTO);
        Task RegisterUserEmailAndLog(string email);
        Task LogOrderPlaced(RewardMessage rewardMessage);
    }
}
