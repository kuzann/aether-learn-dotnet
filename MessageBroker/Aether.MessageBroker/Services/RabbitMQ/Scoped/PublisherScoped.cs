using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Aether.MessageBroker.Services.RabbitMQ.Scoped
{
    public class PublisherScoped
    {
        private ILogger<PublisherScoped> _logger;
        private IModel _channel;

        public PublisherScoped(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<PublisherScoped>();
            var connectionFactory = new ConnectionFactory();
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
            _logger.LogInformation($"Create publisher scoped channel {_channel.ChannelNumber}");
        }

        public void PublishMessage()
        {
            _logger.LogInformation($"Start publishing message on channel {_channel.ChannelNumber}");

            string exchange = "exchange.dotnet";
            _channel.ExchangeDeclare(exchange, ExchangeType.Topic, true, false);
            string queue1 = "queue.dotnet1";
            string queue2 = "queue.dotnet2";
            _DeclareQueue(queue1);
            _DeclareQueue(queue2);
            _channel.QueueBind(queue1, exchange, "dotnet1.*");
            _channel.QueueBind(queue2, exchange, "dotnet2.*");

            _channel.QueuePurge(queue1);
            _channel.QueuePurge(queue2);

            for (int i = 0; i < 5; i++)
            {
                byte[] body = Encoding.UTF8.GetBytes($"[{DateTime.Now.ToString("s")}] This is message {i}");
                _channel.BasicPublish(exchange, i % 2 != 0 ? "dotnet1.publish" : "dotnet2.", null, body);
                Thread.Sleep(500);
            }
              
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
