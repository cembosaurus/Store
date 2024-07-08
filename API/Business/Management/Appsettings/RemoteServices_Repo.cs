using AutoMapper;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;
using Business.Management.Enums;
using Microsoft.IdentityModel.Tokens;



namespace Business.Management.Appsettings
{
    public class RemoteServices_REPO : IRemoteServices_REPO
    {

        private Config_Global_DB _db;


        public RemoteServices_REPO(Config_Global_DB db, IMapper mapper)
        {
            _db = db;
        }




        // Read:

        public bool IsEmpty()
        {
            return _db.Data.RemoteServices.IsNullOrEmpty();
        }

        public ICollection<RemoteService_AS_MODEL> GetAll()
        {
            return _db.Data.RemoteServices.ToList();
        }

        public RemoteService_AS_MODEL GetByName(string name)
        {
            return _db.Data.RemoteServices.FirstOrDefault(s => s.Name == name);
        }

        public RemoteService_AS_MODEL GetByBaseURL(string baseURL)
        {
            return _db.Data.RemoteServices.FirstOrDefault(s => s.Type.Any(t => t.BaseURL.Dev == baseURL || t.BaseURL.Prod == baseURL));
        }

        public ICollection<RemoteService_AS_MODEL> GetByPathName(string pathName)
        {
            return _db.Data.RemoteServices.Where(s => s.Type.Any(t => t.Paths.Any(p => p.Name == pathName))).ToList();
        }


        public ICollection<RemoteService_AS_MODEL> GetByPathRoure(string pathRoute)
        {
            return _db.Data.RemoteServices.Where(s => s.Type.Any(t => t.Paths.Any(p => p.Route == pathRoute))).ToList();
        }


        public ICollection<RemoteService_AS_MODEL> GetByType(string type)
        {
            return _db.Data.RemoteServices.Where(s => s.Type.Any(st => st.Name == type)).ToList();
        }


        public ICollection<string> GetURLsWithPath(TypeOfService type, string pathName, bool isProdEnv)
        {
            return _db.Data.RemoteServices.FindAll(m => m.GetPathByName(type, pathName).IsNullOrEmpty() == false).Select(m => m.GetUrlWithPath(type, pathName, isProdEnv)).ToList();
        }


        public ICollection<string> GetBaseURLs(TypeOfService type, bool isProdEnv)
        {
            return _db.Data.RemoteServices.Select(m => m.GetBaseUrl(type, isProdEnv)).ToList();
        }





        // Write:

        public bool UpdateByName(string name, RemoteService_AS_MODEL serviceURL)
        {
            var url = _db.Data.RemoteServices.FirstOrDefault(s => s.Name == name);

            if (url != null)
            {
                url = serviceURL;

                return true;
            }

            return false;
        }


        public bool UpdateByBaseURL(string baseURL, RemoteService_AS_MODEL serviceURL)
        {
            var url = _db.Data.RemoteServices.FirstOrDefault(s => s.Type.Any(st => st.BaseURL.Dev == baseURL));

            if (url != null)
            {
                url = serviceURL;

                return true;
            }

            return false;
        }




        public bool DeleteByName(string name)
        {
            var url = _db.Data.RemoteServices.FirstOrDefault(s => s.Name == name);

            if (url != null)
            {
                _db.Data.RemoteServices.Remove(url);

                return true;
            }

            return false;
        }


        public bool DeleteByBaseURL(string baseURL)
        {
            var url = _db.Data.RemoteServices.FirstOrDefault(s => s.Type.Any(st => st.BaseURL.Dev == baseURL));

            if (url != null)
            {
                _db.Data.RemoteServices.Remove(url);

                return true;
            }

            return false;
        }





        public bool Initialize(ICollection<RemoteService_AS_MODEL> data)
        {
            if (data.IsNullOrEmpty())
                return false;

            _db.Data.RemoteServices.Clear();

            _db.Data.RemoteServices.AddRange(data);

            return true;
        }
    }
}
