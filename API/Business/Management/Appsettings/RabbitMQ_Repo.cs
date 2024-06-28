using AutoMapper;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;
using static Business.Management.Appsettings.Models.RabbitMQ_AS_MODEL;



namespace Business.Management.Appsettings
{
    public class RabbitMQ_REPO : IRabbitMQ_REPO
    {

        private Config_Global_DB _db;
        private IMapper _mapper;



        public RabbitMQ_REPO(Config_Global_DB db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }




        public RabbitMQ_AS_MODEL Data => _db.Data.RabbitMQ;

        public Env Server(bool isProdEnv) => isProdEnv ? _db.Data.RabbitMQ.Server.Prod : _db.Data.RabbitMQ.Server.Dev;

        public Env Client(bool isProdEnv) => isProdEnv ? _db.Data.RabbitMQ.Client.Prod : _db.Data.RabbitMQ.Client.Dev;

        public void Initialize(RabbitMQ_AS_MODEL rabbitMQ) => _db.Data.RabbitMQ = _mapper.Map<RabbitMQ_AS_MODEL>(rabbitMQ);



        // To Do:
        //
        // Update
        // Create
        // Delete

    }
}
