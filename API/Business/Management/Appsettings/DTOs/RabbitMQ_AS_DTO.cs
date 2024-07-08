namespace Business.Management.Appsettings.DTOs
{
    public class RabbitMQ_AS_DTO
    {

        public Env_DTO Dev { get; set; }
        public Env_DTO Prod { get; set; }


        public class Env_DTO
        {
            public string Host { get; set; }
            public string Port { get; set; }
        }
    }
}
