using AutoMapper;
using Business.Identity.DTOs;

namespace API_Gateway.Profiles
{
    public class InventoryMapperProfile : Profile
    {

        public InventoryMapperProfile()
        {
            // User:
            CreateMap<UserToRegisterDTO, UserReadDTO>();
            CreateMap<UserToLoginDTO, UserReadDTO>();





        }

    }
}
