using AutoMapper;
using Business.Management.Appsettings.DTOs;
using Business.Management.Appsettings.Models;

namespace Services.Inventory.Profiles
{
    public class ManagementProfiles : Profile
    {

        public ManagementProfiles()
        {
            CreateMap<Auth_AS_MODEL, Auth_AS_DTO>();
            CreateMap<Auth_AS_DTO, Auth_AS_MODEL>();

            CreateMap<RabbitMQ_AS_MODEL, RabbitMQ_AS_DTO>();
            CreateMap<RabbitMQ_AS_DTO, RabbitMQ_AS_MODEL>();

            CreateMap<RemoteService_AS_MODEL, RemoteService_AS_DTO>();
            CreateMap<RemoteService_AS_DTO, RemoteService_AS_MODEL>();

            CreateMap<Config_Global_AS_MODEL, Config_Global_AS_DTO>();
            CreateMap<Config_Global_AS_DTO, Config_Global_AS_MODEL>();

        }

    }
}
