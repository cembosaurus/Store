using Business.Management.Appsettings.Models;

namespace Business.Management.Data.Interfaces
{
    public interface IRemoteServicesInfo_DB
    {
        List<Service_Model_AS> URLs { get; set; }
    }
}
