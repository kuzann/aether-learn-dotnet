using StackExchange.Redis;

namespace Aether.Cache
{
    internal class RedisCacheProvider
    {
        public static void Run()
        {
            // [username]:[password]@[host]:[port],ssl=[true|false],defaultDatabase=[db-number]
            //ConfigurationOptions conf = new ConfigurationOptions
            //{
            //    EndPoints = { "localhost:6379" },
            //    //User = "ryan",
            //    Password = "Admin123!"
            //};
            //ConnectionMultiplexer redis2 = ConnectionMultiplexer.Connect(conf);

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379,user=ryan,password=Admin123!");
            IDatabase db = redis.GetDatabase();

            bool changed = false;

            Console.WriteLine(db.StringGet("foo"));

            changed = db.StringSet("foo", "bar");
            Console.WriteLine(changed);
            Console.WriteLine(db.StringGet("foo"));


            changed = db.StringSet("foo", "lorem ipsum", when: When.NotExists);
            Console.WriteLine(changed);
            Console.WriteLine(db.StringGet("foo"));

            //changed = db.StringSet("foo", "lorem ipsum", when: When.Exists);
            //Console.WriteLine(changed);
            //Console.WriteLine(db.StringGet("foo"));

            Console.ReadLine();
        }
    }
}
