using Business.Management.Appsettings.Models;



namespace Business.Management.Appsettings.Interfaces
{
    public interface IRemoteServicesInfo_Repo
    {
        bool DeleteByBaseURL(string baseURL);
        bool DeleteByName(string name);
        List<ServiceURL> GetAllURLs();
        ServiceURL GetByBaseURL(string baseURL);
        ServiceURL GetByName(string name);
        List<ServiceURL> GetByPathName(string pathName);
        List<ServiceURL> GetByPathRoure(string pathRoute);
        List<ServiceURL> GetByType(string type);
        bool InitializeDB(List<ServiceURL> data);
        bool UpdateByBaseURL(string baseURL, ServiceURL serviceURL);
        bool UpdateByName(string name, ServiceURL serviceURL);
    }
}
