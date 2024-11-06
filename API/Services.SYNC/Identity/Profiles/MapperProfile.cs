


using AutoMapper;
using Business.Identity.DTOs;
using Services.Identity.Models;

namespace Services.Identity.Profiles
{
    public class MapperProfile : Profile
    {

        public MapperProfile()
        {
            // User:
            CreateMap<UserToRegisterDTO, AppUser>()
                .ForMember(au => au.UserName, opt => opt.MapFrom(utr => utr.Name));
            CreateMap<UserToLoginDTO, UserReadDTO>();
            CreateMap<AppUser, UserReadDTO>()
                .ForMember(ur => ur.Name, opt => opt.MapFrom(au => au.UserName));
            CreateMap<AppUser, UserAuthDTO>()
                .ForMember(ua => ua.Name, opt => opt.MapFrom(au => au.UserName));

            // Address:
            CreateMap<AddressCreateDTO, Address>();
            CreateMap<AddressUpdateDTO, Address>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Address, AddressReadDTO>();


        }

    }
}
