using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IRemoteServices_REPO
    {
        bool DeleteByBaseURL(string baseURL);
        bool DeleteByName(string name);
        ICollection<RemoteService_AS_MODEL> GetAll();
        RemoteService_AS_MODEL GetByBaseURL(string baseURL);
        RemoteService_AS_MODEL GetByName(string name);
        ICollection<RemoteService_AS_MODEL> GetByPathName(string pathName);
        ICollection<RemoteService_AS_MODEL> GetByPathRoure(string pathRoute);
        ICollection<RemoteService_AS_MODEL> GetByType(string type);
        bool Initialize(ICollection<RemoteService_AS_MODEL> data);
        bool IsEmpty();
        bool UpdateByBaseURL(string baseURL, RemoteService_AS_MODEL serviceURL);
        bool UpdateByName(string name, RemoteService_AS_MODEL serviceURL);
    }
}
