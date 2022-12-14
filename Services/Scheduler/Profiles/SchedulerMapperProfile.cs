using AutoMapper;
using Business.Scheduler.DTOs;
using Scheduler.Models;

namespace Scheduler.Profiles
{
    public class SchedulerMapperProfile : Profile
    {

        public SchedulerMapperProfile()
        {

            CreateMap<CartItemsLockCreateDTO, CartItemLock>();
            CreateMap<CartItemLock, CartItemsLockReadDTO>();



        }
    }
}
