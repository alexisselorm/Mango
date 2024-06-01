using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO?> list = [];
            ResponseDTO response = await _productService.GetAllProducts();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDTO dto)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await _productService.CreateProductAsync(dto);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View();
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDTO response = await _productService.GetProductById(productId);

            if (response != null && response.IsSuccess)
            {
                ProductDTO? model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDTO Product)
        {
            ResponseDTO response = await _productService.DeleteProductAsync(Product.ProductId);

            if (response != null && response.IsSuccess)
            {
                ProductDTO? model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                TempData["success"] = "Product deleted successfully";

                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(Product);
        }

        public async Task<IActionResult> ProductEdit(int ProductId)
        {
            ResponseDTO response = await _productService.GetProductById(ProductId);

            if (response != null && response.IsSuccess)
            {
                ProductDTO? model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        public async Task<IActionResult> ProductDetails(int ProductId)
        {
            ResponseDTO response = await _productService.GetProductById(ProductId);

            if (response != null && response.IsSuccess)
            {
                ProductDTO? model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDTO Product)
        {
            ResponseDTO response = await _productService.UpdateProductAsync(Product);

            if (response != null && response.IsSuccess)
            {
                ProductDTO? model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                TempData["success"] = "Product updated successfully";

                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(Product);
        }
    }
}