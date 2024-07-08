using AutoMapper;
using Business.Management.Appsettings.DTOs;
using Business.Management.Appsettings.Models;
using Business.Management.Models;
using static Business.Management.Appsettings.DTOs.RabbitMQ_AS_DTO;
using static Business.Management.Appsettings.DTOs.RemoteService_AS_DTO;
using static Business.Management.Appsettings.DTOs.RemoteService_AS_DTO.ServiceType_DTO;
using static Business.Management.Appsettings.Models.RabbitMQ_AS_MODEL;
using static Business.Management.Appsettings.Models.RemoteService_AS_MODEL;
using static Business.Management.Appsettings.Models.RemoteService_AS_MODEL.ServiceType;

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
            CreateMap<Env, Env_DTO>();
            CreateMap<Env_DTO, Env>();

            CreateMap<RemoteService_AS_MODEL, RemoteService_AS_DTO>();
            CreateMap<RemoteService_AS_DTO, RemoteService_AS_MODEL>();
            CreateMap<ServiceType, ServiceType_DTO>();
            CreateMap<ServiceType_DTO, ServiceType>();
            CreateMap<SchemeHostPort, SchemeHostPort_DTO>();
            CreateMap<SchemeHostPort_DTO, SchemeHostPort>();
            CreateMap<URLPath, URLPath_DTO>();
            CreateMap<URLPath_DTO, URLPath>();

            CreateMap<Config_Global_AS_MODEL, Config_Global_AS_DTO>();
            CreateMap<Config_Global_AS_DTO, Config_Global_AS_MODEL>();

        }

    }
}
