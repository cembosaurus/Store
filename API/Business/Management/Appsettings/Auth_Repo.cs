using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;



namespace Business.Management.Appsettings
{
    public class Auth_Repo : IAuth_Repo
    {
        private Auth_Model_AS _auth;

        public Auth_Repo(Auth_Model_AS auth)
        {
            _auth = auth;
        }




    }
}
