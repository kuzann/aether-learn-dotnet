using Aether.MessageBroker.Services.RabbitMQ.Scoped;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Aether.MessageBroker.Controllers.RabbitMQ
{
    [Route("api/rmq/scoped")]
    [ApiController]
    public class RmqScopedController : ControllerBase
    {
        private ILoggerFactory _loggerFactory;

        public RmqScopedController(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        [HttpGet("publish")]
        public IActionResult Publish()
        {
            PublisherScoped publisher = new PublisherScoped(_loggerFactory);
            publisher.PublishMessage();
            return Ok();
        }

        [HttpGet("consume")]
        public IActionResult Consume()
        {
            ConsumerScoped consumer = new ConsumerScoped(_loggerFactory);
            consumer.ConsumeMessage();
            return Ok();
        }
    }
}
