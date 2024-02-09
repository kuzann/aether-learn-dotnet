namespace Aether.MessageBroker.Services.RabbitMQ
{
    public class Constants
    {
        public const string QUEUE_SELLER = "queue.dotnet.seller";
        public const string QUEUE_CUSTOMER = "queue.dotnet.customer";

        public const string QUEUE_SELLER_PATTERN = QUEUE_SELLER + ".*";
        public const string QUEUE_CUSTOMER_PATTERN = QUEUE_CUSTOMER + ".*";
    }
}
