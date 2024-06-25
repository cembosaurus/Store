using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Http.Services.Interfaces
{
    public interface IHttpGlobalConfigBroadcast
    {
        Task<IServiceResult<Config_Global_AS_MODEL>> BroadcastUpdate(Config_Global_AS_MODEL globalConfig);
    }
}
