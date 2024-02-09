using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Aether.MessageBroker.Services.RabbitMQ
{
    public class MessageConsumer
    {
        private ILogger<MessageConsumer> _logger;
        private RabbitMQConnectionPool _connectionPool;
        private IModel _channel;

        public MessageConsumer(ILoggerFactory loggerFactor, RabbitMQConnectionPool connectionPool)
        {
            _logger = loggerFactor.CreateLogger<MessageConsumer>();
            _connectionPool = connectionPool;
            _channel = connectionPool.ConsumerChannel;
        }

        public void ConsumeMessage()
        {
            string queueName = "queue.dotnet1";
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, args) =>
            {
                _logger.LogInformation($"Start consuming message on channel {_channel.ChannelNumber}");
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Received {message}");
            };
            _DeclareQueue(queueName);
            _channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);
        }

        private void _DeclareQueue(string queueName)
        {
            var arguments = new Dictionary<string, object>()
            {
                { "x-queue-type", "quorum" }
            };
            _channel.QueueDeclare(queueName, true, false, false, arguments);
        }
    }
}
