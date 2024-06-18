using Business.Exceptions.Interfaces;
using Business.Http;
using Business.Http.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Data;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;



namespace Business.Identity.Http.Services
{
    public class HttpIdentityService : HttpBaseService, IHttpIdentityService
    {

        public HttpIdentityService(IHostingEnvironment env, IExId exId, IAppsettings_PROVIDER appsettingsProvider, IHttpAppClient httpAppClient, IGlobal_Settings_PROVIDER remoteServicesInfoService, IServiceResultFactory resultFact)
            : base(env, exId, appsettingsProvider, httpAppClient, remoteServicesInfoService, resultFact)
        {
            _remoteServiceName = "IdentityService";
            _remoteServicePathName = "Identity";
        }





        public async Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"register";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            return await HTTP_Request_Handler<UserAuthDTO>();
        }



        public async Task<IServiceResult<string>> Login_UserPassword(UserToLoginDTO user)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"login";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            return await HTTP_Request_Handler<string>();
        }



        // NOT USED. ApiKey is used to directly authenticate api service. No JWT necessary:
        public async Task<IServiceResult<string>> Login_ApiKey(string apiKey)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"service/authenticate";
            _requestHeaders.Add("x-api-key", apiKey);

            return await HTTP_Request_Handler<string>();
        }

    }
}
