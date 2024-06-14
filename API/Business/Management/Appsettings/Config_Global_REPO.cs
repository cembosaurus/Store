using Business.Management.Appsettings.Interfaces;
using Business.Management.Data;



namespace Business.Management.Appsettings
{
    public class Config_Global_REPO : IConfig_Global_REPO
    {

        private IRemoteServices_REPO _remoteServices;
        private IAuth_REPO _auth;
        private IRabbitMQ_REPO _rabbitMQ;



        public Config_Global_REPO(Config_Global_DB config_global_DB)
        {
            _remoteServices = new RemoteServices_REPO(config_global_DB);
            _auth = new Auth_REPO(config_global_DB);
            _rabbitMQ = new RabbitMQ_REPO(config_global_DB);
        }



        public IRemoteServices_REPO RemoteServices
        {
            get { return _remoteServices; }
            set { _remoteServices = value; }
        }

        public IAuth_REPO Auth
        {
            get { return _auth; }
            set { _auth = value; }
        }

        public IRabbitMQ_REPO RabbitMQ
        {
            get { return _rabbitMQ; }
            set { _rabbitMQ = value; }
        }


    }
}
