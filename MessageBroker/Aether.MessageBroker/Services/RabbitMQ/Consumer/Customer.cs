using Microsoft.Extensions.Logging;

namespace Aether.MessageBroker.Services.RabbitMQ.Consumer
{
    public class Customer
    {
        private readonly ILogger<Customer> _logger;

        public Customer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Customer>();
        }

        public void ConsumeMessage(string message)
        {
            _logger.LogInformation("Customer consuming message");
        }
    }
}
