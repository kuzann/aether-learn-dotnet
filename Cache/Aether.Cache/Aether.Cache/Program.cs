using StackExchange.Redis;

namespace Aether.Cache
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();

            db.StringSet("foo", "bar");
            Console.WriteLine(db.StringGet("foo"));
        }
    }
}
