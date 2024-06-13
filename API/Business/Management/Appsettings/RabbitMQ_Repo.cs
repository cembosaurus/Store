using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;



namespace Business.Management.Appsettings
{
    public class RabbitMQ_Repo : IRabbitMQ_Repo
    {
        private RabbitMQ_Model_AS _rabbitMQ;

        public RabbitMQ_Repo(RabbitMQ_Model_AS rabbitMQ)
        {
            _rabbitMQ = rabbitMQ;
        }




    }
}
