using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;
using Microsoft.IdentityModel.Tokens;



namespace Business.Management.Appsettings
{
    public class RemoteServices_REPO : IRemoteServices_REPO
    {

        private List<RemoteService_MODEL_AS> _remoteServices;


        public RemoteServices_REPO(Config_Global_DB config_global_DB)
        {
            _remoteServices = config_global_DB.RemoteServices;
        }



        public bool IsEmpty()
        {
            return _remoteServices.IsNullOrEmpty();
        }

        public ICollection<RemoteService_MODEL_AS> GetAll()
        {
            return _remoteServices.ToList();
        }

        public RemoteService_MODEL_AS GetByName(string name)
        {
            return _remoteServices.FirstOrDefault(url => url.Name == name);
        }

        public RemoteService_MODEL_AS GetByBaseURL(string baseURL)
        {
            return _remoteServices.FirstOrDefault(url => url.Type.Any(t => t.BaseURL.Dev == baseURL || t.BaseURL.Prod == baseURL));
        }

        public ICollection<RemoteService_MODEL_AS> GetByPathName(string pathName)
        {
            return _remoteServices.Where(url => url.Type.Any(t => t.Paths.Any(p => p.Name == pathName))).ToList();
        }


        public ICollection<RemoteService_MODEL_AS> GetByPathRoure(string pathRoute)
        {
            return _remoteServices.Where(url => url.Type.Any(t => t.Paths.Any(p => p.Route == pathRoute))).ToList();
        }


        public ICollection<RemoteService_MODEL_AS> GetByType(string type)
        {
            return _remoteServices.Where(url => url.Type.Any(st => st.Name == type)).ToList();
        }


        public bool UpdateByName(string name, RemoteService_MODEL_AS serviceURL)
        {
            var url = _remoteServices.FirstOrDefault(url => url.Name == name);

            if (url != null)
            {
                url = serviceURL;

                return true;
            }

            return false;
        }


        public bool UpdateByBaseURL(string baseURL, RemoteService_MODEL_AS serviceURL)
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



        public bool InitializeDB(ICollection<RemoteService_MODEL_AS> data)
        {
            if (data.IsNullOrEmpty())
                return false;

            _remoteServices.AddRange(data);

            return true;
        }
    }
}
