using Business.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Services.Interfaces;



namespace Business.Http.Services
{
    public class HttpGlobalConfigBroadcast : IHttpGlobalConfigBroadcast
    {

        private IGlobal_Settings_PROVIDER _globalSettings_Provider;



        public HttpGlobalConfigBroadcast(IServiceResultFactory resultFact, IGlobal_Settings_PROVIDER globalSettings_Provider)
        {
            _globalSettings_Provider = globalSettings_Provider;
        }





        public async Task<IServiceResult<Config_Global_AS_MODEL>> BroadcastUpdate(Config_Global_AS_MODEL globalConfig)
        {
            return null;
        }
    }
}
