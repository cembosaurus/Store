using Business.Management.Appsettings.Models;
using static Business.Management.Appsettings.Models.RabbitMQ_AS_MODEL;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IRabbitMQ_REPO
    {
        RabbitMQ_AS_MODEL Data { get; }

        Env Client(bool isProdEnv);
        Env Server(bool isProdEnv);
        void Initialize(RabbitMQ_AS_MODEL rabbitMQ);
    }
}
