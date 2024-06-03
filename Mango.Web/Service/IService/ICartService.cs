using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDTO?> GetCartByUserIdAsync(string userId);
        Task<ResponseDTO?> UpsertCartAsync(CartDTO dto);
        Task<ResponseDTO?> RemoveFromCartAsync(int cartDetailsid);
        Task<ResponseDTO?> ApplyCouponAsync(CartDTO dto);
        Task<ResponseDTO?> EmailCart(CartDTO dto);

    }
}
