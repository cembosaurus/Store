using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;



namespace Business.Management.Appsettings
{
    public class Auth_REPO : IAuth_REPO
    {
        private Auth_MODEL_AS _auth;

        public Auth_REPO(Config_Global_DB config_global_DB)
        {
            _auth = config_global_DB.Auth;
        }


        public Auth_MODEL_AS Get => _auth;


    }
}
