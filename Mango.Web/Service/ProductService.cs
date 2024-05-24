using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {

            _baseService = baseService;

        }
        public async Task<ResponseDTO?> CreateProductAsync(ProductDTO ProductDTO)
        {
            return await _baseService.sendAsync(
           new()
           {
               ApiType = StaticDetails.ApiType.POST,
               Url = StaticDetails.ProductAPIUrl + "/api/product",
               Data = ProductDTO
           });
        }

        public async Task<ResponseDTO?> DeleteProductAsync(int id)
        {
            return await _baseService.sendAsync(
           new()
           {
               ApiType = StaticDetails.ApiType.DELETE,
               Url = StaticDetails.ProductAPIUrl + "/api/product/" + id
           });
        }

        public async Task<ResponseDTO?> GetAllProducts()
        {
            return await _baseService.sendAsync(
                new()
                {
                    ApiType = StaticDetails.ApiType.GET,
                    Url = StaticDetails.ProductAPIUrl + "/api/product"
                });
        }

        public async Task<ResponseDTO?> GetProductAsync(string code)
        {
            return await _baseService.sendAsync(
           new()
           {
               ApiType = StaticDetails.ApiType.GET,
               Url = StaticDetails.ProductAPIUrl + "/api/product/" + code
           });
        }

        public async Task<ResponseDTO?> GetProductById(int id)
        {
            return await _baseService.sendAsync(
           new()
           {
               ApiType = StaticDetails.ApiType.GET,
               Url = StaticDetails.ProductAPIUrl + "/api/product/" + id
           });
        }

        public async Task<ResponseDTO?> UpdateProductAsync(ProductDTO ProductDTO)
        {
            return await _baseService.sendAsync(
       new()
       {
           ApiType = StaticDetails.ApiType.PUT,
           Url = StaticDetails.ProductAPIUrl + "/api/product/",
           Data = ProductDTO
       });
        }
    }
}
