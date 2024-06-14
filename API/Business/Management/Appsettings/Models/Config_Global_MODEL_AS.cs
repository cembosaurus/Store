namespace Business.Management.Appsettings.Models
{
    public class Config_Global_MODEL_AS
    {
        public List<RemoteService_MODEL_AS> RemoteServices { get; set; } = new List<RemoteService_MODEL_AS>();
        public Auth_MODEL_AS Auth { get; set; } = new Auth_MODEL_AS();
        public RabbitMQ_MODEL_AS RabbitMQ { get; set; } = new RabbitMQ_MODEL_AS() { Port = "", Host = ""};

    }
}
