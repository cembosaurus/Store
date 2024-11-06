



using AutoMapper;
using Business.Identity.DTOs;

namespace API_Gateway.Profiles
{
    public class MapperProfile : Profile
    {

        public MapperProfile()
        {
            // User:
            CreateMap<UserToRegisterDTO, UserReadDTO>();
            CreateMap<UserToLoginDTO, UserReadDTO>();





        }

    }
}
