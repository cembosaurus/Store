using AutoMapper;
using Business.API_Gateway.DTOs.CatalogueItem;
using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.DTOs.Item;
using Business.Inventory.DTOs.ItemPrice;
using Business.Ordering.DTOs;
using Business.Payment.DTOs;
using Services.Ordering.Models;

namespace Ordering.Profiles
{
    public class CartMapperProfile : Profile
    {

        public CartMapperProfile()
        {
            // Cart:

            CreateMap<Cart, CartReadDTO>();
            CreateMap<CartUpdateDTO, Cart>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CartItems, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CartItemUpdateDTO, CartItem>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Cart, CartReadDTOForUser>();


            // Cart-Items:
            CreateMap<CartItem, CartItemReadDTO>();
            CreateMap<ItemReadDTO, CartItemReadDTO>();
            CreateMap<ItemPriceReadDTO, CartItemReadDTO>();
            CreateMap<CatalogueItemReadDTO, CatalogueItemReadDTO_View>()
                .ForMember(dest => dest.SalesPrice, cfg => cfg.MapFrom(src => src.ItemPrice.SalePrice))
                .ForMember(dest => dest.Name, cfg => cfg.MapFrom(src => src.Item.Name));

            // Order:

            CreateMap<Order, OrderReadDTO>();
            CreateMap<Order, OrderPaymentCreateDTO>();
            CreateMap<OrderCreateDTO, Order>();
            CreateMap<OrderUpdateDTO, Order>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<OrderDetails, OrderDetailsReadDTO>();
            CreateMap<OrderDetailsCreateDTO, OrderDetails>();
            CreateMap<OrderDetailsUpdateDTO, OrderDetails>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


            // 
        }

    }
}
