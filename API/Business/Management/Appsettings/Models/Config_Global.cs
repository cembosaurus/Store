namespace Business.Management.Appsettings.Models
{
    public class Config_Global
    {
        public IEnumerable<ServiceURL_AS> RemoteServices { get; set; }
        public Auth_AS Auth { get; set; }
        public RabbitMQ_AS RabbitMQ { get; set; }

    }
}
