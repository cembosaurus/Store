using Business.Exceptions.Interfaces;
using Business.Http;
using Business.Http.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;



namespace Business.Identity.Http.Services
{
    public class HttpUserService : HttpBaseService, IHttpUserService
    {

        public HttpUserService(IHostingEnvironment env, IExId exId, IAppsettings_Provider appsettingsService, IHttpAppClient httpAppClient, IRemoteServices_Provider remoteServices_Provider, IServiceResultFactory resultFact) :
        base(env, exId, appsettingsService, httpAppClient, remoteServices_Provider, resultFact)
        {
            _remoteServiceName = "IdentityService";
        }





        public async Task<IServiceResult<IEnumerable<string>>> EditUserRoles(int id, IEnumerable<string> roles)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"/user/{id}/changeroles";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(roles), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<string>>();
        }



        public async Task<IServiceResult<IEnumerable<UserReadDTO>>> GetAllUsers()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/user";


            return await HTTP_Request_Handler<IEnumerable<UserReadDTO>>();
        }



        public async Task<IServiceResult<IEnumerable<UserWithRolesReadDTO>>> GetAllUsersWithRoles()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/user/withroles";


            return await HTTP_Request_Handler<IEnumerable<UserWithRolesReadDTO>>();
        }



        public async Task<IServiceResult<UserReadDTO>> GetCurrentUser()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/user/current";


            return await HTTP_Request_Handler<UserReadDTO>();
        }



        public async Task<IServiceResult<UserReadDTO>> GetUserById(int id)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/user/{id}";


            return await HTTP_Request_Handler<UserReadDTO>();
        }



        public async Task<IServiceResult<UserReadDTO>> GetUserByName(string name)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/user/name/{name}";


            return await HTTP_Request_Handler<UserReadDTO>();
        }



        public async Task<IServiceResult<UserWithRolesReadDTO>> GetUserWithRoles(int id)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/user/{id}/withroles";


            return await HTTP_Request_Handler<UserWithRolesReadDTO>();
        }
    }
}
