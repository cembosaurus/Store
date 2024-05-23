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




        public List<ServiceURL_AS> GetAllURLs()
        { 
            return _remoteServicesInfo_DB.URLs.ToList();
        }

        public ServiceURL_AS GetByName(string name)
        {
            return _remoteServicesInfo_DB.URLs.FirstOrDefault(url => url.Name == name);
        }

        public ServiceURL_AS GetByBaseURL(string baseURL)
        {
            return _remoteServicesInfo_DB.URLs.FirstOrDefault(url => url.Type.Any(t => t.BaseURL.Dev == baseURL || t.BaseURL.Prod == baseURL));
        }

        public List<ServiceURL_AS> GetByPathName(string pathName)
        {
            return _remoteServicesInfo_DB.URLs.FindAll(url => url.Type.Any(t => t.Paths.Any(p => p.Name == pathName)));
        }


        public List<ServiceURL_AS> GetByPathRoure(string pathRoute)
        {
            return _remoteServicesInfo_DB.URLs.FindAll(url => url.Type.Any(t => t.Paths.Any(p => p.Route == pathRoute)));
        }


        public List<ServiceURL_AS> GetByType(string type)
        {
            return _remoteServicesInfo_DB.URLs.FindAll(url => url.Type.Any(st => st.Name == type));
        }


        public bool UpdateByName(string name, ServiceURL_AS serviceURL)
        { 
            var url = _remoteServicesInfo_DB.URLs.FirstOrDefault(url => url.Name == name);

            if (url != null) 
            {
                url = serviceURL;

                return true;
            }

            return false;
        }


        public bool UpdateByBaseURL(string baseURL, ServiceURL_AS serviceURL)
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



        public bool InitializeDB(List<ServiceURL_AS> data)
        {
            if(data.IsNullOrEmpty())
                return false;

            _remoteServicesInfo_DB.URLs = data;

            return true;
        }

    }
}
