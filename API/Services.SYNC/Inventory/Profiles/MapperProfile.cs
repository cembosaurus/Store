using AutoMapper;
using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.DTOs.CatalogueItem.AccessoryItem;
using Business.Inventory.DTOs.CatalogueItem.SimilarProductItem;
using Business.Inventory.DTOs.Item;
using Business.Inventory.DTOs.ItemPrice;
using Inventory.Models;

namespace Services.Inventory.Profiles
{
    public class MapperProfile : Profile
    {

        public MapperProfile()
        {
            // Item:
            CreateMap<ItemCreateDTO, Item>();
            CreateMap<ItemUpdateDTO, Item>()
                .ForMember(dest => dest.Id, cfg => cfg.Ignore())
                .ForMember(dest => dest.Description, cfg => cfg.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Description = src.Description == null ? dest.Description : src.Description ?? dest.Description;
                })
                .ForMember(dest => dest.PhotoURL, cfg => cfg.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.PhotoURL = src.PhotoURL == null ? dest.PhotoURL : src.PhotoURL ?? dest.PhotoURL;
                })
                .ForAllMembers(cfg => cfg.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Item, ItemReadDTO>();
            CreateMap<Item, CatalogueItem>()
                .ForMember(dest => dest.ItemId, cfg => cfg.MapFrom(src => src.Id))
                .ForMember(dest => dest.Description, cfg => cfg.Ignore());

            // CatalogueItem:
            CreateMap<CatalogueItemCreateDTO, CatalogueItem>()
                .ForMember(dest => dest.ItemPrice, cfg => cfg.MapFrom(src => src.ItemPrice));
            CreateMap<CatalogueItemUpdateDTO, CatalogueItem>()
                .ForMember(dest => dest.ItemPrice, cfg => cfg.MapFrom(src => src.ItemPrice))
                .ForAllMembers(cfg => cfg.Condition((src, dest, srcMember) => srcMember != null));        // UPDATE: keep source data on NULL update
            CreateMap<CatalogueItem, CatalogueItemReadDTO>()
                .ForMember(dest => dest.ItemPrice, cfg => cfg.MapFrom(src => src.ItemPrice));
            CreateMap<CatalogueItem, CatalogueItemReadDTOWithExtras>()
                .ForMember(dest => dest.AccessoryItems, cfg => cfg.Ignore())
                .ForMember(dest => dest.SimilarProductItems, cfg => cfg.Ignore());

            //CreateMap<CatalogueItem, ExtrasReadDTO>()
            //    .ForMember(dest => dest.Accessories, cfg => cfg.MapFrom(src => src.Accessories))
            //    .ForMember(dest => dest.ItemPrice, cfg => cfg.MapFrom(src => src.ItemPrice));



            // ItemPrice:
            CreateMap<ItemPriceCreateDTO, ItemPrice>();
            CreateMap<ItemPriceUpdateDTO, ItemPrice>()                                                    // UPDATE: ignore null and 0 for SalesPrice on update
                .ForMember(dest => dest.SalePrice, cfg => cfg.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.SalePrice = src.SalePrice == 0 ? dest.SalePrice : src.SalePrice ?? dest.SalePrice;
                })
                .ForAllMembers(cfg => cfg.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ItemPrice, ItemPriceReadDTO>();

            // AccessoryItem:
            CreateMap<AccessoryItem, AccessoryItemReadDTO>();

            // SimilarProductItem:
            CreateMap<SimilarProductItem, SimilarProductItemReadDTO>();

            // CatalogueItemWithExtras:
            CreateMap<CatalogueItem, CatalogueItemReadDTOWithExtras>();




        }

    }
}
