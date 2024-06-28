using AutoMapper;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;

namespace Business.Management.Appsettings
{
    public class Auth_REPO : IAuth_REPO
    {

        private Config_Global_DB _db;
        private IMapper _mapper;



        public Auth_REPO(Config_Global_DB db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }




        public Auth_AS_MODEL Data => _db.Data.Auth;

        public string Apikey => _db.Data.Auth.ApiKey;

        public string JWTKey => _db.Data.Auth.JWTKey;

        public void Initialize(Auth_AS_MODEL auth) => _db.Data.Auth = _mapper.Map<Auth_AS_MODEL>(auth);
        



        // To Do:
        //
        // Update
        // Create
        // Delete


    }
}
