using Business.Management.Appsettings.Models;
using Business.Management.Data.Interfaces;



namespace Business.Management.Data
{
    // singleton

    public class Appsettings_DB : IAppsettings_DB
    {

        private Config_Global_Data _config_global_DATA;


        public Appsettings_DB()
        {
            _config_global_DATA = new Config_Global_Data();
            _config_global_DATA.RemoteServices = new List<Service_Model_AS>();
            _config_global_DATA.Auth = new Auth_Model_AS();
            _config_global_DATA.RabbitMQ = new RabbitMQ_Model_AS();
        }




        public ICollection<Service_Model_AS> RemoteServices
        {
            get => _config_global_DATA.RemoteServices;
            set => _config_global_DATA.RemoteServices = value;
        }


        public Auth_Model_AS Auth
        {
            get => _config_global_DATA.Auth;
            set => _config_global_DATA.Auth = value;
        }


        public RabbitMQ_Model_AS RabbitMQ
        {
            get => _config_global_DATA.RabbitMQ;
            set => _config_global_DATA.RabbitMQ = value;
        }

    }
}
