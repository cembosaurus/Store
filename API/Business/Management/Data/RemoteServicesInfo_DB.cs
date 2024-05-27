using Business.Management.Appsettings.Models;
using Business.Management.Data.Interfaces;



namespace Business.Management.Data
{
    public class RemoteServicesInfo_DB : IRemoteServicesInfo_DB
    {
        // Singleton, holding all service's URLs during the app lifetime:
        private List<Service_Model_AS> _URLs;


        public RemoteServicesInfo_DB()
        {
            _URLs = new List<Service_Model_AS>();
        }


        public List<Service_Model_AS> URLs
        {
            get => _URLs;
            set => _URLs = value;
        }
    }
}
