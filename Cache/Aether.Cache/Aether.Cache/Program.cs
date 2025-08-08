namespace Aether.Cache
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MemoryCacheProvider.Run();
            RedisCacheProvider.Run();
        }
    }
}
