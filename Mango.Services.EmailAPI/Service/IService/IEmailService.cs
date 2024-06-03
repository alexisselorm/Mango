using Mango.Services.EmailAPI.Models.DTO;

namespace Mango.Services.EmailAPI.Service.IService
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDTO cartDTO);
    }
}
