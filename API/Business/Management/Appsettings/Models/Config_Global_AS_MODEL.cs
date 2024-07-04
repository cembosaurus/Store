using Newtonsoft.Json;

namespace Business.Management.Appsettings.Models
{
    public class Config_Global_AS_MODEL
    {
        [JsonProperty("RemoteServices")]
        public List<RemoteService_AS_MODEL> RemoteServices { get; set; } = new List<RemoteService_AS_MODEL>();
        [JsonProperty("Auth")]
        public Auth_AS_MODEL Auth { get; set; } = new Auth_AS_MODEL();
        [JsonProperty("RabbitMQ")]
        public RabbitMQ_AS_MODEL RabbitMQ { get; set; } = new RabbitMQ_AS_MODEL();

    }
}
