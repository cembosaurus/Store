using Business.AMQP.AMQPClient.Interfaces;
using Business.AMQP.Custom;
using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Inventory.Publisher.AMQPServices.Interfaces;
using System.Text.Json;

namespace Inventory.Publisher.AMQPServices
{
    public class AMQPItemService : IAMQPItemService
    {

        private readonly IAMQClient _amqpItemClient;
        private readonly IServiceResultFactory _resultFact;
        private IHttpContextAccessor _accessor;
        private readonly string _failMessage = "FAILED to get result from AMQP client !";


        public AMQPItemService(IAMQClient amqpItemClient, IServiceResultFactory resultFact, IHttpContextAccessor accessor, IConfiguration config)
        {
            _amqpItemClient = amqpItemClient;
            _amqpItemClient.RequestQueueName = config.GetSection("RabbitMQ:ItemRequestQueueName").Value;
            _resultFact = resultFact;
            _accessor = accessor;
        }





        public async Task<IServiceResult<IEnumerable<ItemReadDTO>>> GetItems(IEnumerable<int> itemIds = default)
        {
            _accessor.HttpContext?.Request.Headers.Add("RequestMethod", itemIds == null || itemIds.Any() 
                ? ((int)RequestMethods.Item.Get).ToString() 
                : ((int)RequestMethods.Item.GetAll).ToString());

            var param = JsonSerializer.Serialize(new { itemIds });

            var response = await _amqpItemClient.Request(param);

            if (!response.Status)
                return _resultFact.Result<IEnumerable<ItemReadDTO>>(null, false, $"{_failMessage}: {response.Message}");

            var result = JsonSerializer.Deserialize<ServiceResult<IEnumerable<ItemReadDTO>>>(response.Data);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> GetItemById(int id)
        {
            _accessor.HttpContext?.Request.Headers.Add("RequestMethod", ((int)RequestMethods.Item.GetById).ToString());

            var param = JsonSerializer.Serialize(new { id });

            var response = await _amqpItemClient.Request(param);

            if (!response.Status)
                return _resultFact.Result<ItemReadDTO>(null, false, $"{_failMessage}: {response.Message}");

            var result = JsonSerializer.Deserialize<ServiceResult<ItemReadDTO>>(response.Data);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> AddItem(ItemCreateDTO item)
        {
            _accessor.HttpContext?.Request.Headers.Add("RequestMethod", ((int)RequestMethods.Item.Add).ToString());

            var param = JsonSerializer.Serialize(new { item });

            var response = await _amqpItemClient.Request(param);

            if (!response.Status)
                return _resultFact.Result<ItemReadDTO>(null, false, $"{_failMessage}: {response.Message}");

            var result = JsonSerializer.Deserialize<ServiceResult<ItemReadDTO>>(response.Data);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> UpdateItem(int id, ItemUpdateDTO item)
        {
            _accessor.HttpContext?.Request.Headers.Add("RequestMethod", ((int)RequestMethods.Item.Update).ToString());

            var param = JsonSerializer.Serialize(new { id, item });

            var response = await _amqpItemClient.Request(param);

            if (!response.Status)
                return _resultFact.Result<ItemReadDTO>(null, false, $"{_failMessage}: {response.Message}");

            var result = JsonSerializer.Deserialize<ServiceResult<ItemReadDTO>>(response.Data);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> DeleteItem(int id)
        {
            _accessor.HttpContext?.Request.Headers.Add("RequestMethod", ((int)RequestMethods.Item.Remove).ToString());

            var param = JsonSerializer.Serialize(new { id });

            var response = await _amqpItemClient.Request(param);

            if (!response.Status)
                return _resultFact.Result<ItemReadDTO>(null, false, $"{_failMessage}: {response.Message}");

            var result = JsonSerializer.Deserialize<ServiceResult<ItemReadDTO>>(response.Data);

            return result;
        }


    }
}
