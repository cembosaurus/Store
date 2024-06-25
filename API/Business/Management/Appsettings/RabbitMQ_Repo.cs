using AutoMapper;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;



namespace Business.Management.Appsettings
{
    public class RabbitMQ_REPO : IRabbitMQ_REPO
    {

        private RabbitMQ_AS_MODEL _rabbitMQ;
        private IMapper _mapper;



        public RabbitMQ_REPO(RabbitMQ_AS_MODEL rabbitMQ, IMapper mapper)
        {
            _rabbitMQ = rabbitMQ;
            _mapper = mapper;
        }




        public RabbitMQ_AS_MODEL Get => _rabbitMQ;

        public void Initi8alize(RabbitMQ_AS_MODEL rabbitMQ) => _rabbitMQ = _mapper.Map<RabbitMQ_AS_MODEL>(rabbitMQ);



        // To Do:
        //
        // Update
        // Create
        // Delete

    }
}
