using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Aether.MessageBroker.Services.RabbitMQ.Scoped
{
    public class ConsumerScoped
    {
        private ILogger<ConsumerScoped> _logger;
        private IModel _channel;

        public ConsumerScoped(ILoggerFactory loggerFactor)
        {
            _logger = loggerFactor.CreateLogger<ConsumerScoped>();
            var connectionFactory = new ConnectionFactory();
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
            _logger.LogInformation($"Create consumer scoped channel {_channel.ChannelNumber}");
        }

        public void ConsumeMessage()
        {
            string queueName = "queue.dotnet1";
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                _logger.LogInformation($"Start consuming message on channel {_channel.ChannelNumber}");
                var body = ea.Body.ToArray();
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
