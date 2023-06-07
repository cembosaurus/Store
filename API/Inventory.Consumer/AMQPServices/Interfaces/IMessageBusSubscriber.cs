using Business.Libraries.ServiceResult.Interfaces;

namespace Inventory.Consumer.AMQPServices.Interfaces
{
    public interface IMessageBusSubscriber
    {
        bool IsConnectedToRabbitMQServer { get; }
        void Dispose();
        Task<IServiceResult<bool>> Connect(CancellationToken cancellationToken);
    }
}
