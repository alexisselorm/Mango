using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClient;
        public CouponService(IHttpClientFactory httpClient)
        {

            _httpClient = httpClient;

        }
        public async Task<CouponDTO> GetCoupon(string couponCode)
        {
            var client = _httpClient.CreateClient("Coupon");
            var response = await client.GetAsync($"api/Coupon/GetByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(resp.Result));

            }
            return new CouponDTO();
        }


    }
}
