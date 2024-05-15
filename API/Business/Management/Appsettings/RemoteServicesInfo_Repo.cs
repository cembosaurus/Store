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




        public List<ServiceURL> GetAllURLs()
        { 
            return _remoteServicesInfo_DB.URLs.ToList();
        }

        public ServiceURL GetByName(string name)
        {
            return _remoteServicesInfo_DB.URLs.FirstOrDefault(url => url.Name == name);
        }

        public ServiceURL GetByBaseURL(string baseURL)
        {
            return _remoteServicesInfo_DB.URLs.FirstOrDefault(url => url.Type.Any(t => t.BaseURL.Dev == baseURL || t.BaseURL.Prod == baseURL));
        }

        public List<ServiceURL> GetByPathName(string pathName)
        {
            return _remoteServicesInfo_DB.URLs.FindAll(url => url.Type.Any(t => t.Paths.Any(p => p.Name == pathName)));
        }


        public List<ServiceURL> GetByPathRoure(string pathRoute)
        {
            return _remoteServicesInfo_DB.URLs.FindAll(url => url.Type.Any(t => t.Paths.Any(p => p.Route == pathRoute)));
        }


        public List<ServiceURL> GetByType(string type)
        {
            return _remoteServicesInfo_DB.URLs.FindAll(url => url.Type.Any(st => st.Name == type));
        }


        public bool UpdateByName(string name, ServiceURL serviceURL)
        { 
            var url = _remoteServicesInfo_DB.URLs.FirstOrDefault(url => url.Name == name);

            if (url != null) 
            {
                url = serviceURL;

                return true;
            }

            return false;
        }


        public bool UpdateByBaseURL(string baseURL, ServiceURL serviceURL)
        {
            var url = _remoteServicesInfo_DB.URLs.FirstOrDefault(url => url.Type.Any(st => st.BaseURL.Dev == baseURL));

            if (url != null)
            {
                url = serviceURL;

                return true;
            }

            return false;
        }


        public bool DeleteByName(string name) 
        {
            var url = _remoteServicesInfo_DB.URLs.FirstOrDefault(url => url.Name == name);

            if (url != null)
            {
                _remoteServicesInfo_DB.URLs.Remove(url);

                return true;
            }

            return false;
        }


        public bool DeleteByBaseURL(string baseURL)
        {
            var url = _remoteServicesInfo_DB.URLs.FirstOrDefault(url => url.Type.Any(st => st.BaseURL.Dev == baseURL));

            if (url != null)
            {
                _remoteServicesInfo_DB.URLs.Remove(url);

                return true;
            }

            return false;
        }



        public bool InitializeDB(List<ServiceURL> data)
        {
            if(data.IsNullOrEmpty())
                return false;

            _remoteServicesInfo_DB.URLs = data;

            return true;
        }

    }
}
