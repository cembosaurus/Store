using Business.Management.Appsettings.Models;



namespace Business.Management.Appsettings.Interfaces
{
    public interface IRemoteServicesInfo_Repo
    {
        bool DeleteByBaseURL(string baseURL);
        bool DeleteByName(string name);
        List<ServiceURL_AS> GetAllURLs();
        ServiceURL_AS GetByBaseURL(string baseURL);
        ServiceURL_AS GetByName(string name);
        List<ServiceURL_AS> GetByPathName(string pathName);
        List<ServiceURL_AS> GetByPathRoure(string pathRoute);
        List<ServiceURL_AS> GetByType(string type);
        bool InitializeDB(List<ServiceURL_AS> data);
        bool UpdateByBaseURL(string baseURL, ServiceURL_AS serviceURL);
        bool UpdateByName(string name, ServiceURL_AS serviceURL);
    }
}
