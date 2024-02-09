using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Aether.MessageBroker.Services.RabbitMQ
{
    /// <summary>
    /// https://stackoverflow.com/questions/10407760/is-there-a-performance-difference-between-pooling-connections-or-channels-in-rab
    /// https://stackoverflow.com/questions/44358076/should-i-close-the-channel-connection-after-every-publish
    /// 1 connection per application and 1 channel per thread.
    /// </summary>
    public class RabbitMQConnectionPool
    {
        private readonly ILogger<RabbitMQConnectionPool> _logger;
        private readonly IConnectionFactory connectionFactory;
        private readonly IConnection connection;

        public IModel PublisherChannel { get; private set; }
        public IModel ConsumerChannel { get; private set; }

        public RabbitMQConnectionPool(ILoggerFactory loggerFactory)
        {
            _logger = new Logger<RabbitMQConnectionPool>(loggerFactory);
            connectionFactory = new ConnectionFactory(); // hostname is localhost by default
            connection = connectionFactory.CreateConnection();
            PublisherChannel = connection.CreateModel();
            ConsumerChannel = connection.CreateModel();
        }

        public void InitializeConsumer()
        {
            if (ConsumerChannel != null && ConsumerChannel.IsOpen)
            {
                var consumer = new EventingBasicConsumer(ConsumerChannel);
                consumer.Received += (model, args) =>
                {
                    _logger.LogInformation($"Start consuming message on channel {ConsumerChannel.ChannelNumber}");
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation($"Exchange: [{args.Exchange}] Routing Key: [{args.RoutingKey}] Body [{message}]");
                };
                ConsumerChannel.BasicConsume(Constants.QUEUE_CUSTOMER, false, consumer);
                ConsumerChannel.BasicConsume(Constants.QUEUE_SELLER, false, consumer);
            }
        }
    }
}
