using API.Dto;
using AutoMapper;
using Core.Entities;
using Core.Entities.Basket;
using Core.Entities.Identity;
using Core.Entities.Order;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>().ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PictureIUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<Photo, PhotoToReturnDto>().ForMember(d => d.PictureUrl, o => o.MapFrom<PhotoUrlResolver>());
            CreateMap<ProductCreateDto, Product>();
            CreateMap<AddressHistory, AddressHistoryDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<Order, OrderToReturnDto>().ForMember(o => o.DeliveryMethod, oT => oT.MapFrom(o => o.PaymentMethod));
            CreateMap<OrderItem, OrderItemDto>()
            .ForMember(o => o.ProductId, ot => ot.MapFrom(o => o.ItemOrdered.ProductItemId))
            .ForMember(o => o.ProductName, ot => ot.MapFrom(o => o.ItemOrdered.ProductName))
            .ForMember(o => o.PictureUrl, ot => ot.MapFrom<OrderItemUrlResolver>());
        }
    }
}