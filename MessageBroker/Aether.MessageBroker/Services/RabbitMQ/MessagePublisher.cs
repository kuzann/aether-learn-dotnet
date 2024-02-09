using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Aether.MessageBroker.Services.RabbitMQ
{
    public class MessagePublisher
    {
        private ILogger<MessagePublisher> _logger;
        private RabbitMQConnectionPool _connectionPool;
        private IModel _channel;

        public MessagePublisher(ILoggerFactory logger, RabbitMQConnectionPool connectionPool)
        {
            _logger = logger.CreateLogger<MessagePublisher>();
            _connectionPool = connectionPool;
            _channel = connectionPool.PublisherChannel;
        }

        public void PublishMessage()
        {
            _logger.LogInformation($"Start publishing message on channel {_channel.ChannelNumber}");

            string exchange = "exchange.dotnet";
            _channel.ExchangeDeclare(exchange, ExchangeType.Topic, true, false);

            string queue1 = Constants.QUEUE_CUSTOMER;
            string queue2 = Constants.QUEUE_SELLER;
            DeclareQueue(queue1);
            DeclareQueue(queue2);
            _channel.QueueBind(queue1, exchange, Constants.QUEUE_CUSTOMER_PATTERN);
            _channel.QueueBind(queue2, exchange, Constants.QUEUE_SELLER_PATTERN);

            _channel.QueuePurge(queue1);
            _channel.QueuePurge(queue2);

            _channel.BasicPublish(exchange, Constants.QUEUE_SELLER_PATTERN.Replace("*", "create"), null, GetBodyMessage());
            _channel.BasicPublish(exchange, Constants.QUEUE_SELLER_PATTERN.Replace("*", "update"), null, GetBodyMessage());
            _channel.BasicPublish(exchange, Constants.QUEUE_CUSTOMER_PATTERN.Replace("*", "create"), null, GetBodyMessage());
            _channel.BasicPublish(exchange, Constants.QUEUE_CUSTOMER_PATTERN.Replace("*", "update"), null, GetBodyMessage());
            _channel.BasicPublish(exchange, "queue.dotnet.payment.create", null, GetBodyMessage());
        }

        private void DeclareQueue(string queueName)
        {
            var arguments = new Dictionary<string, object>()
            {
                { "x-queue-type", "quorum" }
            };
            _channel.QueueDeclare(queueName, true, false, false, arguments);
        }

        private byte[] GetBodyMessage()
        {
            Guid id = Guid.NewGuid();
            _logger.LogInformation($"Body message with id: {id}");
            return Encoding.UTF8.GetBytes(id.ToString());
        }
    }
}
