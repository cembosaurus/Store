using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Services.Interfaces
{
    public interface IRemoteServices_PROVIDER
    {
        IServiceResult<RemoteService_MODEL_AS> GetServiceByBaseURL(string baseURL);
        IServiceResult<RemoteService_MODEL_AS> GetServiceByName(string name);
        IServiceResult<string> GetServiceURL_WithPath(RemoteService_MODEL_AS serviceUrl, string pathName);
        IServiceResult<string> GetServiceURL_WithPath(string serviceName, string pathName);
        IServiceResult<bool> IsEmpty();
        Task<IServiceResult<IEnumerable<RemoteService_MODEL_AS>>> ReLoad();
        IServiceResult<IEnumerable<RemoteService_MODEL_AS>> Update(IEnumerable<RemoteService_MODEL_AS> servicesModels);

    }
}
