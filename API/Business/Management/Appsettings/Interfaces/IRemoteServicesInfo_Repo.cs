using Business.Management.Appsettings.Models;



namespace Business.Management.Appsettings.Interfaces
{
    public interface IRemoteServicesInfo_Repo
    {
        bool DeleteByBaseURL(string baseURL);
        bool DeleteByName(string name);
        ICollection<Service_Model_AS> GetAll();
        Service_Model_AS GetByBaseURL(string baseURL);
        Service_Model_AS GetByName(string name);
        ICollection<Service_Model_AS> GetByPathName(string pathName);
        ICollection<Service_Model_AS> GetByPathRoure(string pathRoute);
        ICollection<Service_Model_AS> GetByType(string type);
        bool InitializeDB(ICollection<Service_Model_AS> data);
        bool IsEmpty();
        bool UpdateByBaseURL(string baseURL, Service_Model_AS serviceURL);
        bool UpdateByName(string name, Service_Model_AS serviceURL);
    }
}
