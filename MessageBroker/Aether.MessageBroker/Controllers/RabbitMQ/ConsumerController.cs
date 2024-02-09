using Aether.MessageBroker.Services.RabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace Aether.MessageBroker.Controllers.RabbitMQ
{
    [Route("api/rmq/consumer")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        private MessageConsumer _consumerService;

        public ConsumerController(MessageConsumer consumerService)
        {
            _consumerService = consumerService;
        }

        [HttpGet("consume")]
        public IActionResult Consume()
        {
            _consumerService.ConsumeMessage();
            return Ok();
        }
    }
}
