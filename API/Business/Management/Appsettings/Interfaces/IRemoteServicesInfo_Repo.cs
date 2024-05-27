using Business.Management.Appsettings.Models;



namespace Business.Management.Appsettings.Interfaces
{
    public interface IRemoteServicesInfo_Repo
    {
        bool DeleteByBaseURL(string baseURL);
        bool DeleteByName(string name);
        List<Service_Model_AS> GetAllURLs();
        Service_Model_AS GetByBaseURL(string baseURL);
        Service_Model_AS GetByName(string name);
        List<Service_Model_AS> GetByPathName(string pathName);
        List<Service_Model_AS> GetByPathRoure(string pathRoute);
        List<Service_Model_AS> GetByType(string type);
        bool InitializeDB(List<Service_Model_AS> data);
        bool UpdateByBaseURL(string baseURL, Service_Model_AS serviceURL);
        bool UpdateByName(string name, Service_Model_AS serviceURL);
    }
}
