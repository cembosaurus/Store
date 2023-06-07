using AutoMapper;
using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.DTOs.CatalogueItem.AccessoryItem;
using Business.Inventory.DTOs.CatalogueItem.SimilarProductItem;
using Business.Inventory.DTOs.Item;
using Business.Inventory.DTOs.ItemPrice;
using Inventory.Consumer.Models;

namespace Inventory.Consumer.Profiles
{
    public class InventoryMapperProfile : Profile
    {

        public InventoryMapperProfile()
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


        }

    }
}
