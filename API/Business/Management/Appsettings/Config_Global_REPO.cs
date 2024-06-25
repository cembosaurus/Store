using AutoMapper;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;



namespace Business.Management.Appsettings
{
    public class Config_Global_REPO
    {        
        
        private Config_Global_AS_MODEL _globalConfig;
        private IMapper _mapper;


        public Config_Global_REPO(Config_Global_DB config_global_DB, IMapper mapper)
        {
            _globalConfig = config_global_DB.Data;
            RemoteServices = new RemoteServices_REPO(config_global_DB.Data.RemoteServices, mapper);
            Auth = new Auth_REPO(config_global_DB.Data.Auth, mapper);
            RabbitMQ = new RabbitMQ_REPO(config_global_DB.Data.RabbitMQ, mapper);
            _mapper = mapper;
        }




        public IRemoteServices_REPO RemoteServices;

        public IAuth_REPO Auth;

        public IRabbitMQ_REPO RabbitMQ;

        public void Initialize(Config_Global_AS_MODEL globalConfig) => _globalConfig = _mapper.Map<Config_Global_AS_MODEL>(globalConfig);


    }
}
