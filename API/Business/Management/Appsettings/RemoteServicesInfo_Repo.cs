using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data.Interfaces;
using Microsoft.IdentityModel.Tokens;



namespace Business.Management.Appsettings
{
    public class RemoteServicesInfo_Repo : IRemoteServicesInfo_Repo
    {

        ICollection<Service_Model_AS> _remoteServices;



        public RemoteServicesInfo_Repo(IAppsettings_DB appsettings_DB)
        {
            _remoteServices = appsettings_DB.RemoteServices;
        }



        //....................................................... make is Appsettings_Repo !!!!! ...........................................................
        //....................................................... make is Appsettings_Repo !!!!! ...........................................................

        //....................................................... make is Appsettings_Repo !!!!! ...........................................................

        //....................................................... make is Appsettings_Repo !!!!! ...........................................................

        //....................................................... make is Appsettings_Repo !!!!! ...........................................................

        //....................................................... make is Appsettings_Repo !!!!! ...........................................................



        public bool IsEmpty()
        { 
            return _remoteServices.IsNullOrEmpty();
        }

        public ICollection<Service_Model_AS> GetAll()
        { 
            return _remoteServices.ToList();
        }

        public Service_Model_AS GetByName(string name)
        {
            return _remoteServices.FirstOrDefault(url => url.Name == name);
        }

        public Service_Model_AS GetByBaseURL(string baseURL)
        {
            return _remoteServices.FirstOrDefault(url => url.Type.Any(t => t.BaseURL.Dev == baseURL || t.BaseURL.Prod == baseURL));
        }

        public ICollection<Service_Model_AS> GetByPathName(string pathName)
        {
            return _remoteServices.Where(url => url.Type.Any(t => t.Paths.Any(p => p.Name == pathName))).ToList();
        }


        public ICollection<Service_Model_AS> GetByPathRoure(string pathRoute)
        {
            return _remoteServices.Where(url => url.Type.Any(t => t.Paths.Any(p => p.Route == pathRoute))).ToList();
        }


        public ICollection<Service_Model_AS> GetByType(string type)
        {
            return _remoteServices.Where(url => url.Type.Any(st => st.Name == type)).ToList();
        }


        public bool UpdateByName(string name, Service_Model_AS serviceURL)
        { 
            var url = _remoteServices.FirstOrDefault(url => url.Name == name);

            if (url != null) 
            {
                url = serviceURL;

                return true;
            }

            return false;
        }


        public bool UpdateByBaseURL(string baseURL, Service_Model_AS serviceURL)
        {
            var url = _remoteServices.FirstOrDefault(url => url.Type.Any(st => st.BaseURL.Dev == baseURL));

            if (url != null)
            {
                url = serviceURL;

                return true;
            }

            return false;
        }


        public bool DeleteByName(string name) 
        {
            var url = _remoteServices.FirstOrDefault(url => url.Name == name);

            if (url != null)
            {
                _remoteServices.Remove(url);

                return true;
            }

            return false;
        }


        public bool DeleteByBaseURL(string baseURL)
        {
            var url = _remoteServices.FirstOrDefault(url => url.Type.Any(st => st.BaseURL.Dev == baseURL));

            if (url != null)
            {
                _remoteServices.Remove(url);

                return true;
            }

            return false;
        }



        public bool InitializeDB(ICollection<Service_Model_AS> data)
        {
            if(data.IsNullOrEmpty())
                return false;

            _remoteServices = data;

            return true;
        }

    }
}
