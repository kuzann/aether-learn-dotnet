using Microsoft.Extensions.Logging;

namespace Aether.MessageBroker.Services.RabbitMQ.Consumer
{
    public class Seller
    {
        private readonly ILogger<Seller> _logger;

        public Seller(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Seller>();
        }

        public void ConsumeMessage(string message)
        {

        }
    }
}
