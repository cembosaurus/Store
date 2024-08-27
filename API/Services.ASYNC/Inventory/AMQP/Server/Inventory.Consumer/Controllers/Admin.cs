using Inventory.Consumer.AMQPServices;
using Inventory.Consumer.AMQPServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Consumer.Controllers
{
    [Authorize]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class Admin : ControllerBase
    {
        private readonly MessageBusSubscriber _messageBusSubscriber;

        public Admin(MessageBusSubscriber messageBusSubscriber)
        {
            _messageBusSubscriber = messageBusSubscriber;
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("amqp/start")]
        public async Task<ActionResult> ConnectToRabbitMQServer(CancellationToken token)
        {
            if (_messageBusSubscriber.IsConnectedToRabbitMQServer)
                return Ok("RabbitMQ server is already connected !");

            var result = await _messageBusSubscriber.Connect(token);

            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
