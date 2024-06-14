using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IRemoteServices_REPO
    {
        bool DeleteByBaseURL(string baseURL);
        bool DeleteByName(string name);
        ICollection<RemoteService_MODEL_AS> GetAll();
        RemoteService_MODEL_AS GetByBaseURL(string baseURL);
        RemoteService_MODEL_AS GetByName(string name);
        ICollection<RemoteService_MODEL_AS> GetByPathName(string pathName);
        ICollection<RemoteService_MODEL_AS> GetByPathRoure(string pathRoute);
        ICollection<RemoteService_MODEL_AS> GetByType(string type);
        bool InitializeDB(ICollection<RemoteService_MODEL_AS> data);
        bool IsEmpty();
        bool UpdateByBaseURL(string baseURL, RemoteService_MODEL_AS serviceURL);
        bool UpdateByName(string name, RemoteService_MODEL_AS serviceURL);
    }
}
