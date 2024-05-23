using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDTO?> sendAsync(RequestDTO requestDTO, bool withBearer = true);
    }
}
