using Newtonsoft.Json;

namespace Business.Management.Appsettings.Models
{
    public class Auth_AS_MODEL
    {
        [JsonProperty("JWTKey")]
        public string JWTKey { get; set; } = "";
        [JsonProperty("ApiKey")]
        public string ApiKey { get; set; } = "";
    }
}
