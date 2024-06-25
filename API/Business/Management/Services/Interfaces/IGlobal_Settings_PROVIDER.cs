using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Services.Interfaces
{
    public interface IGlobal_Settings_PROVIDER
    {
        IServiceResult<RemoteService_AS_MODEL> GetRemoteServiceByBaseURL(string baseURL);
        IServiceResult<RemoteService_AS_MODEL> GetRemoteServiceByName(string name);
        IServiceResult<IEnumerable<RemoteService_AS_MODEL>> GetRemoteServices_WithGlobalConfig();
        IServiceResult<string> GetRemoteServiceURL_WithPath(RemoteService_AS_MODEL serviceUrl, string pathName);
        IServiceResult<string> GetRemoteServiceURL_WithPath(string serviceName, string pathName);
        IServiceResult<bool> IsEmpty_RemoteServiceModels();
        Task<IServiceResult<Config_Global_AS_MODEL>> ReLoad();
        Task<IServiceResult<IEnumerable<RemoteService_AS_MODEL>>> ReLoadRemoteServices();
        IServiceResult<Config_Global_AS_MODEL> Update(Config_Global_AS_MODEL config);
        IServiceResult<IEnumerable<RemoteService_AS_MODEL>> UpdateRemoteServiceModels(IEnumerable<RemoteService_AS_MODEL> servicesModels);
    }
}
