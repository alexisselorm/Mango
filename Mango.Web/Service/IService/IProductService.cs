using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDTO?> GetProductAsync(string name);
        Task<ResponseDTO?> GetAllProducts();
        Task<ResponseDTO?> GetProductById(int id);
        Task<ResponseDTO?> CreateProductAsync(ProductDTO ProductDTO);
        Task<ResponseDTO?> UpdateProductAsync(ProductDTO ProductDTO);
        Task<ResponseDTO?> DeleteProductAsync(int id);
    }
}
