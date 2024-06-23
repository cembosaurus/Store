using Business.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Services.Interfaces;



namespace Business.Http.Services
{
    public class HttpGlobalSettingsBroadcast : IHttpGlobalSettingsBroadcast
    {

        private IGlobal_Settings_PROVIDER _globalSettings_Provider;



        public HttpGlobalSettingsBroadcast(IServiceResultFactory resultFact, IGlobal_Settings_PROVIDER globalSettings_Provider)
        {
            _globalSettings_Provider = globalSettings_Provider;
        }





        public async Task<IServiceResult<IEnumerable<RemoteService_MODEL_AS>>> UpdateRemoteServices()
        {
            return null;
        }
    }
}
