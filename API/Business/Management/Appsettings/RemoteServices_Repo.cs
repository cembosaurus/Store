using AutoMapper;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Microsoft.IdentityModel.Tokens;



namespace Business.Management.Appsettings
{
    public class RemoteServices_REPO : IRemoteServices_REPO
    {

        private List<RemoteService_AS_MODEL> _remoteServices;


        public RemoteServices_REPO(List<RemoteService_AS_MODEL> remoteServices, IMapper mapper)
        {
            _remoteServices = remoteServices;
        }



        public bool IsEmpty()
        {
            return _remoteServices.IsNullOrEmpty();
        }

        public ICollection<RemoteService_AS_MODEL> GetAll()
        {
            return _remoteServices.ToList();
        }

        public RemoteService_AS_MODEL GetByName(string name)
        {
            return _remoteServices.FirstOrDefault(s => s.Name == name);
        }

        public RemoteService_AS_MODEL GetByBaseURL(string baseURL)
        {
            return _remoteServices.FirstOrDefault(s => s.Type.Any(t => t.BaseURL.Dev == baseURL || t.BaseURL.Prod == baseURL));
        }

        public ICollection<RemoteService_AS_MODEL> GetByPathName(string pathName)
        {
            return _remoteServices.Where(s => s.Type.Any(t => t.Paths.Any(p => p.Name == pathName))).ToList();
        }


        public ICollection<RemoteService_AS_MODEL> GetByPathRoure(string pathRoute)
        {
            return _remoteServices.Where(s => s.Type.Any(t => t.Paths.Any(p => p.Route == pathRoute))).ToList();
        }


        public ICollection<RemoteService_AS_MODEL> GetByType(string type)
        {
            return _remoteServices.Where(s => s.Type.Any(st => st.Name == type)).ToList();
        }





        public bool UpdateByName(string name, RemoteService_AS_MODEL serviceURL)
        {
            var url = _remoteServices.FirstOrDefault(s => s.Name == name);

            if (url != null)
            {
                url = serviceURL;

                return true;
            }

            return false;
        }


        public bool UpdateByBaseURL(string baseURL, RemoteService_AS_MODEL serviceURL)
        {
            var url = _remoteServices.FirstOrDefault(s => s.Type.Any(st => st.BaseURL.Dev == baseURL));

            if (url != null)
            {
                url = serviceURL;

                return true;
            }

            return false;
        }




        public bool DeleteByName(string name)
        {
            var url = _remoteServices.FirstOrDefault(s => s.Name == name);

            if (url != null)
            {
                _remoteServices.Remove(url);

                return true;
            }

            return false;
        }


        public bool DeleteByBaseURL(string baseURL)
        {
            var url = _remoteServices.FirstOrDefault(s => s.Type.Any(st => st.BaseURL.Dev == baseURL));

            if (url != null)
            {
                _remoteServices.Remove(url);

                return true;
            }

            return false;
        }





        public bool Initialize(ICollection<RemoteService_AS_MODEL> data)
        {
            if (data.IsNullOrEmpty())
                return false;

            _remoteServices.Clear();

            _remoteServices.AddRange(data);

            return true;
        }
    }
}
