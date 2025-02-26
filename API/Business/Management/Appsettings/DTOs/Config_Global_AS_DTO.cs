using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.DTOs
{
    public class Config_Global_AS_DTO
    {
        public IEnumerable<RemoteService_AS_DTO> RemoteServices { get; set; }
        public Auth_AS_DTO Auth { get; set; }
        public RabbitMQ_AS_DTO RabbitMQ { get; set; }
        public Persistence_AS_MODEL Persistence { get; set; }

    }
}
