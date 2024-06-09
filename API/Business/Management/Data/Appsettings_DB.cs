using Business.Management.Appsettings.Models;
using Business.Management.Data.Interfaces;



namespace Business.Management.Data
{
    // singleton

    public class Appsettings_DB : IAppsettings_DB
    {

        private Config_Global_Model_AS _globalConfig;


        public Appsettings_DB()
        {
            _globalConfig = new Config_Global_Model_AS();
            _globalConfig.RemoteServices = new List<Service_Model_AS>();
            _globalConfig.Auth = new Auth_Model_AS();
            _globalConfig.RabbitMQ = new RabbitMQ_Model_AS();
        }




        public ICollection<Service_Model_AS> RemoteServices
        {
            get => _globalConfig.RemoteServices;
            set => _globalConfig.RemoteServices = value;
        }


        public Auth_Model_AS Auth
        {
            get => _globalConfig.Auth;
            set => _globalConfig.Auth = value;
        }


        public RabbitMQ_Model_AS RabbitMQ
        {
            get => _globalConfig.RabbitMQ;
            set => _globalConfig.RabbitMQ = value;
        }

    }
}
