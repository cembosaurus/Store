using AutoMapper;
using Business.Scheduler.DTOs;
using Scheduler.Models;

namespace Scheduler.Profiles
{
    public class MapperProfile : Profile
    {

        public MapperProfile()
        {

            CreateMap<CartItemsLockCreateDTO, CartItemLock>();
            CreateMap<CartItemLock, CartItemsLockReadDTO>();



        }
    }
}
