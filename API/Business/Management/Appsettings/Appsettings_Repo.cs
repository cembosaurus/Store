using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data.Interfaces;



namespace Business.Management.Appsettings
{
    public class Appsettings_Repo : IAppsettings_Repo
    {

        private RemoteServices_Repo _remoteServices;
        private Auth_Repo _auth;
        private RabbitMQ_Repo _rabbitMQ;



        public Appsettings_Repo(IAppsettings_DB appsettings_DB)
        {
            _remoteServices = new RemoteServices_Repo(appsettings_DB.RemoteServices ?? new List<Service_Model_AS>());
            _auth = new Auth_Repo(appsettings_DB.Auth ?? new Auth_Model_AS());
            _rabbitMQ = new RabbitMQ_Repo(appsettings_DB.RabbitMQ ?? new RabbitMQ_Model_AS());
        }



        public RemoteServices_Repo RemoteServices
        {
            get { return _remoteServices; }
            set { _remoteServices = value; }
        }

        public Auth_Repo Auth
        {
            get { return _auth; }
            set { _auth = value; }
        }

        public RabbitMQ_Repo RabbitMQ
        {
            get { return _rabbitMQ; }
            set { _rabbitMQ = value; }
        }


    }
}
