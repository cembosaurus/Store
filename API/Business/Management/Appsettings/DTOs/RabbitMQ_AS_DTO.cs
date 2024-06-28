namespace Business.Management.Appsettings.DTOs
{
    public class RabbitMQ_AS_DTO
    {

        public Env Dev { get; set; }
        public Env Prod { get; set; }


        public class Env
        {
            public string Host { get; set; }
            public string Port { get; set; }
        }
    }
}
