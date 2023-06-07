using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult.Interfaces;

namespace Business.AMQP.AMQPClient.Interfaces
{
    public interface IAMQClient
    {
        string RequestQueueName { set; }

        Task<IServiceResult<string>> Request(string reqParam);
    }
}
