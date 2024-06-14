using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IRabbitMQ_REPO
    {
        RabbitMQ_MODEL_AS Get { get; }
    }
}
