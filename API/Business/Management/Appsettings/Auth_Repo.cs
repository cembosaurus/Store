using AutoMapper;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;



namespace Business.Management.Appsettings
{
    public class Auth_REPO : IAuth_REPO
    {

        private Auth_AS_MODEL _auth;
        private IMapper _mapper;



        public Auth_REPO(Auth_AS_MODEL auth, IMapper mapper)
        {
            _auth = auth;
            _mapper = mapper;
        }




        public Auth_AS_MODEL Get => _auth;

        public void Initi8alize(Auth_AS_MODEL auth) => _auth = _mapper.Map<Auth_AS_MODEL>(auth);
        



        // To Do:
        //
        // Update
        // Create
        // Delete


    }
}
