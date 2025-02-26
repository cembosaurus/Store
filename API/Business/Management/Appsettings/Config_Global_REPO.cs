using AutoMapper;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;



namespace Business.Management.Appsettings
{
    public class Config_Global_REPO : IConfig_Global_REPO
    {
        private Config_Global_DB _db;
        private RemoteServices_REPO _remoteServices;
        private Auth_REPO _auth;
        private RabbitMQ_REPO _rabbitMQ;
        private Persistence_REPO _persistence;
        private IMapper _mapper;



        public Config_Global_REPO(Config_Global_DB db, IMapper mapper)
        {
            _db = db;
            _remoteServices = new RemoteServices_REPO(_db, mapper);
            _auth = new Auth_REPO(_db, mapper);
            _rabbitMQ = new RabbitMQ_REPO(_db, mapper);
            _persistence = new Persistence_REPO(_db, mapper);
            _mapper = mapper;
        }


        public Config_Global_AS_MODEL GlobalConfig => _db.Data;

        public IRemoteServices_REPO RemoteServices => _remoteServices;

        public IAuth_REPO Auth => _auth;

        public IRabbitMQ_REPO RabbitMQ => _rabbitMQ;

        public Persistence_REPO Persistence => _persistence;

        public void Initialize(Config_Global_AS_MODEL globalConfig) => _db.Data = _mapper.Map<Config_Global_AS_MODEL>(globalConfig);


    }
}
