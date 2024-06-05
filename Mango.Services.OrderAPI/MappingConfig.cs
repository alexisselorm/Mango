using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.DTO;

namespace Mango.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeader, OrderHeaderDTO>().ReverseMap();
                config.CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
