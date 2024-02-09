using Aether.MessageBroker.Services.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace Aether.MessageBroker.Controllers.RabbitMQ
{
    [Route("api/rmq/publisher")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private MessagePublisher _publisherService;

        public PublisherController(MessagePublisher publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet("publish")]
        public IActionResult Publish()
        {
            _publisherService.PublishMessage();
            return Ok();
        }
    }
}
