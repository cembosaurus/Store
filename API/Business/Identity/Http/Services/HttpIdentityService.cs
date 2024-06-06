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
    public class HttpIdentityService : HttpBaseService, IHttpIdentityService
    {

        public HttpIdentityService(IHostingEnvironment env, IExId exId, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient, IRemoteServicesInfo_Provider remoteServicesInfoService, IServiceResultFactory resultFact)
            : base(env, exId, appsettingsService, httpAppClient, remoteServicesInfoService, resultFact)
        {
            _remoteServiceName = "IdentityService";
        }





        public async Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"identity/register";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            return await HTTP_Request_Handler<UserAuthDTO>();
        }



        public async Task<IServiceResult<string>> Login_UserPassword(UserToLoginDTO user)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"identity/login";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            return await HTTP_Request_Handler<string>();
        }



        public async Task<IServiceResult<string>> Login_ApiKey(string apiKey)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"identity/service/authenticate";
            _requestHeaders.Add("ApiKey", apiKey);

            return await HTTP_Request_Handler<string>();
        }

    }
}
