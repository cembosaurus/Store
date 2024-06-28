namespace Business.Management.Appsettings.Models
{
    public class RabbitMQ_AS_MODEL
    {

        public App Server { get; set; } = new App();
        public App Client { get; set; } = new App();


        public class Env
        {
            public string Host { get; set; } = "";
            public string Port { get; set; } = "";
        }

        public class App
        {
            public Env Dev { get; set; } = new Env();
            public Env Prod { get; set; } = new Env();
        }
    }
}
