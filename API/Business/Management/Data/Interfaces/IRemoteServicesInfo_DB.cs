using Business.Management.Appsettings.Models;

namespace Business.Management.Data.Interfaces
{
    public interface IRemoteServicesInfo_DB
    {
        List<ServiceURL> URLs { get; set; }
    }
}
