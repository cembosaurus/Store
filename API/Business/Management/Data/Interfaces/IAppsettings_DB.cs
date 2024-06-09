using Business.Management.Appsettings.Models;

namespace Business.Management.Data.Interfaces
{
    public interface IAppsettings_DB
    {
        Auth_Model_AS Auth { get; set; }
        RabbitMQ_Model_AS RabbitMQ { get; set; }
        ICollection<Service_Model_AS> RemoteServices { get; set; }
    }
}
