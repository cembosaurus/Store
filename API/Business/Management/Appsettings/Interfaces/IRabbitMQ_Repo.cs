using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IRabbitMQ_REPO
    {
        RabbitMQ_AS_MODEL Get { get; }

        void Initi8alize(RabbitMQ_AS_MODEL rabbitMQ);
    }
}
