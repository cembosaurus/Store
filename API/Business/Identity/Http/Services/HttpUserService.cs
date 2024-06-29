using Business.Exceptions.Interfaces;
using Business.Http.Clients;
using Business.Http.Services;
using Business.Identity.DTOs;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Business.Identity.Http.Services
{
    public class HttpUserService : HttpBaseService, IHttpUserService
    {

        public HttpUserService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact)
            : base(accessor, env, exId, httpAppClient, remoteServices_Provider, resultFact)
        {
            _remoteServiceName = "IdentityService";
            _remoteServicePathName = "User";
        }





        public async Task<IServiceResult<IEnumerable<string>>> EditUserRoles(int id, IEnumerable<string> roles)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"{id}/changeroles";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(roles), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<string>>();
        }



        public async Task<IServiceResult<IEnumerable<UserReadDTO>>> GetAllUsers()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"";


            return await HTTP_Request_Handler<IEnumerable<UserReadDTO>>();
        }



        public async Task<IServiceResult<IEnumerable<UserWithRolesReadDTO>>> GetAllUsersWithRoles()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"withroles";


            return await HTTP_Request_Handler<IEnumerable<UserWithRolesReadDTO>>();
        }



        public async Task<IServiceResult<UserReadDTO>> GetCurrentUser()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"current";


            return await HTTP_Request_Handler<UserReadDTO>();
        }



        public async Task<IServiceResult<UserReadDTO>> GetUserById(int id)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{id}";


            return await HTTP_Request_Handler<UserReadDTO>();
        }



        public async Task<IServiceResult<UserReadDTO>> GetUserByName(string name)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"name/{name}";


            return await HTTP_Request_Handler<UserReadDTO>();
        }



        public async Task<IServiceResult<UserWithRolesReadDTO>> GetUserWithRoles(int id)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{id}/withroles";


            return await HTTP_Request_Handler<UserWithRolesReadDTO>();
        }
    }
}
