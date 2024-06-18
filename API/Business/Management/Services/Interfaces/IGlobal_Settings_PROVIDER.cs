using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Services.Interfaces
{
    public interface IGlobal_Settings_PROVIDER
    {
        IServiceResult<RemoteService_MODEL_AS> GetRemoteServiceByBaseURL(string baseURL);
        IServiceResult<RemoteService_MODEL_AS> GetRemoteServiceByName(string name);
        IServiceResult<string> GetRemoteServiceURL_WithPath(RemoteService_MODEL_AS serviceUrl, string pathName);
        IServiceResult<string> GetRemoteServiceURL_WithPath(string serviceName, string pathName);
        IServiceResult<bool> IsEmpty();
        Task<IServiceResult<IEnumerable<RemoteService_MODEL_AS>>> ReLoadRemoteServices();
        IServiceResult<IEnumerable<RemoteService_MODEL_AS>> UpdateRemoteServices(IEnumerable<RemoteService_MODEL_AS> servicesModels);

    }
}
