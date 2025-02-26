using Newtonsoft.Json;

namespace Business.Management.Appsettings.Models
{
    public class Config_Global_AS_MODEL
    {
        [JsonProperty("RemoteServices")]
        public List<RemoteService_AS_MODEL> RemoteServices { get; set; } = new ();
        [JsonProperty("Auth")]
        public Auth_AS_MODEL Auth { get; set; } = new ();
        [JsonProperty("RabbitMQ")]
        public RabbitMQ_AS_MODEL RabbitMQ { get; set; } = new ();
        [JsonProperty("Persistence")]
        public Persistence_AS_MODEL Persistence { get; set; } = new ();

    }
}
