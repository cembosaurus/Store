using Newtonsoft.Json;

namespace Business.Management.Appsettings.Models
{
    public class RabbitMQ_AS_MODEL
    {
        [JsonProperty("Server")]
        public App Server { get; set; } = new App();
        [JsonProperty("Client")]
        public App Client { get; set; } = new App();


        public class Env
        {
            [JsonProperty("Host")]
            public string Host { get; set; } = "";
            [JsonProperty("Port")]
            public string Port { get; set; } = "";
        }

        public class App
        {
            [JsonProperty("Dev")]
            public Env Dev { get; set; } = new Env();
            [JsonProperty("Prod")]
            public Env Prod { get; set; } = new Env();
        }
    }
}
