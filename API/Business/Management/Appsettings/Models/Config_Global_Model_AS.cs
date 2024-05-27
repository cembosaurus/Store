namespace Business.Management.Appsettings.Models
{
    public class Config_Global_Model_AS
    {
        public IEnumerable<Service_Model_AS> RemoteServices { get; set; }
        public Auth_Model_AS Auth { get; set; }
        public RabbitMQ_Model_AS RabbitMQ { get; set; }

    }
}
