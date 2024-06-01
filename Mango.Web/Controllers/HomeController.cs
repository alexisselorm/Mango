using IdentityModel;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
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

        [HttpPost]
        [ActionName("ProductDetails")]
        [Authorize]
        public async Task<IActionResult> ProductDetails(ProductDTO productDTO)
        {
            CartDTO cartDTO = new CartDTO()
            {
                CartHeader = new CartHeaderDTO
                {
                    UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDTO cartDetailsDTO = new CartDetailsDTO()
            {
                Count = productDTO.Count,
                ProductId = productDTO.ProductId,
            };

            List<CartDetailsDTO> cartDetailsDTOs = new() { cartDetailsDTO };
            cartDTO.CartDetails = cartDetailsDTOs;

            ResponseDTO response = await _cartService.UpsertCartAsync(cartDTO);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item added to cart";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDTO);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
