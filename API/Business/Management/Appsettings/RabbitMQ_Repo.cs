using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;



namespace Business.Management.Appsettings
{
    public class RabbitMQ_REPO : IRabbitMQ_REPO
    {
        private RabbitMQ_MODEL_AS _rabbitMQ;

        public RabbitMQ_REPO(Config_Global_DB config_global_DB)
        {
            _rabbitMQ = config_global_DB.RabbitMQ;
        }



        public RabbitMQ_MODEL_AS Get => _rabbitMQ;

    }
}
