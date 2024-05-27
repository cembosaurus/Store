using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data.Interfaces;
using Microsoft.IdentityModel.Tokens;



namespace Business.Management.Appsettings
{
    public class RemoteServicesInfo_Repo : IRemoteServicesInfo_Repo
    {

        private IRemoteServicesInfo_DB _remoteServicesInfo_DB;



        public RemoteServicesInfo_Repo(IRemoteServicesInfo_DB remoteServicesInfo_DB)
        {
            _remoteServicesInfo_DB = remoteServicesInfo_DB;
        }




        public bool IsEmpty()
        { 
            return _remoteServicesInfo_DB.Services.IsNullOrEmpty();
        }

        public List<Service_Model_AS> GetAll()
        { 
            return _remoteServicesInfo_DB.Services.ToList();
        }

        public Service_Model_AS GetByName(string name)
        {
            return _remoteServicesInfo_DB.Services.FirstOrDefault(url => url.Name == name);
        }

        public Service_Model_AS GetByBaseURL(string baseURL)
        {
            return _remoteServicesInfo_DB.Services.FirstOrDefault(url => url.Type.Any(t => t.BaseURL.Dev == baseURL || t.BaseURL.Prod == baseURL));
        }

        public List<Service_Model_AS> GetByPathName(string pathName)
        {
            return _remoteServicesInfo_DB.Services.FindAll(url => url.Type.Any(t => t.Paths.Any(p => p.Name == pathName)));
        }


        public List<Service_Model_AS> GetByPathRoure(string pathRoute)
        {
            return _remoteServicesInfo_DB.Services.FindAll(url => url.Type.Any(t => t.Paths.Any(p => p.Route == pathRoute)));
        }


        public List<Service_Model_AS> GetByType(string type)
        {
            return _remoteServicesInfo_DB.Services.FindAll(url => url.Type.Any(st => st.Name == type));
        }


        public bool UpdateByName(string name, Service_Model_AS serviceURL)
        { 
            var url = _remoteServicesInfo_DB.Services.FirstOrDefault(url => url.Name == name);

            if (url != null) 
            {
                url = serviceURL;

                return true;
            }

            return false;
        }


        public bool UpdateByBaseURL(string baseURL, Service_Model_AS serviceURL)
        {
            var url = _remoteServicesInfo_DB.Services.FirstOrDefault(url => url.Type.Any(st => st.BaseURL.Dev == baseURL));

            if (url != null)
            {
                url = serviceURL;

                return true;
            }

            return false;
        }


        public bool DeleteByName(string name) 
        {
            var url = _remoteServicesInfo_DB.Services.FirstOrDefault(url => url.Name == name);

            if (url != null)
            {
                _remoteServicesInfo_DB.Services.Remove(url);

                return true;
            }

            return false;
        }


        public bool DeleteByBaseURL(string baseURL)
        {
            var url = _remoteServicesInfo_DB.Services.FirstOrDefault(url => url.Type.Any(st => st.BaseURL.Dev == baseURL));

            if (url != null)
            {
                _remoteServicesInfo_DB.Services.Remove(url);

                return true;
            }

            return false;
        }



        public bool InitializeDB(List<Service_Model_AS> data)
        {
            if(data.IsNullOrEmpty())
                return false;

            _remoteServicesInfo_DB.Services = data;

            return true;
        }

    }
}
