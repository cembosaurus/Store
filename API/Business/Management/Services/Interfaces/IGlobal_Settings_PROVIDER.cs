using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Services.Interfaces
{
    public interface IGlobal_Settings_PROVIDER
    {
        IServiceResult<RemoteService_MODEL_AS> GetRemoteServiceByBaseURL(string baseURL);
        IServiceResult<RemoteService_MODEL_AS> GetRemoteServiceByName(string name);
        IServiceResult<IEnumerable<RemoteService_MODEL_AS>> GetRemoteServices_WithHTTPClient();
        IServiceResult<string> GetRemoteServiceURL_WithPath(RemoteService_MODEL_AS serviceUrl, string pathName);
        IServiceResult<string> GetRemoteServiceURL_WithPath(string serviceName, string pathName);
        IServiceResult<bool> IsEmpty();
        IServiceResult<bool> IsRemoteServiceHttpClient(string name);
        Task<IServiceResult<IEnumerable<RemoteService_MODEL_AS>>> ReLoadRemoteServices();
        IServiceResult<IEnumerable<RemoteService_MODEL_AS>> UpdateRemoteServiceModels(IEnumerable<RemoteService_MODEL_AS> servicesModels);
    }
}
