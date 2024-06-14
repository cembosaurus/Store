using Business.Management.Appsettings.Models;



namespace Business.Management.Data
{
    // singleton

    public class Config_Global_DB
    {

        private Config_Global_MODEL_AS _config_global_MODEL_AS = new Config_Global_MODEL_AS();


        public List<RemoteService_MODEL_AS> RemoteServices
        {
            get => _config_global_MODEL_AS.RemoteServices;
            set => _config_global_MODEL_AS.RemoteServices = value;
        }


        public Auth_MODEL_AS Auth
        {
            get => _config_global_MODEL_AS.Auth;
            set => _config_global_MODEL_AS.Auth = value;
        }


        public RabbitMQ_MODEL_AS RabbitMQ
        {
            get => _config_global_MODEL_AS.RabbitMQ;
            set => _config_global_MODEL_AS.RabbitMQ = value;
        }

    }
}
