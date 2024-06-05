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
                config.CreateMap<OrderHeaderDTO, CartHeaderDTO>()
                .ForMember(dest => dest.TotalAmount, u => u.MapFrom(src => src.TotalAmount)).ReverseMap();
                config.CreateMap<CartDetailsDTO, OrderDetailDTO>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price)).ReverseMap();

                config.CreateMap<OrderDetailDTO, CartDetailsDTO>();

                config.CreateMap<OrderHeader, OrderHeaderDTO>().ReverseMap();
                config.CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();


            });
            return mappingConfig;
        }
    }
}
