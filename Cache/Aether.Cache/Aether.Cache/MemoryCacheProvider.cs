using Microsoft.Extensions.Caching.Memory;

namespace Aether.Cache
{
    internal static class MemoryCacheProvider
    {
        public static void Run()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            string key = "myKey";
            string value = "myValue";

            try
            {
                AddToCache(memoryCache, key, value); // First add should succeed
                AddToCache(memoryCache, key, value); // Second add should throw exception
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        static void AddToCache(IMemoryCache cache, string key, object value)
        {
            if (cache.TryGetValue(key, out _))
            {
                throw new InvalidOperationException($"Key '{key}' already exists in the cache.");
            }

            cache.Set(key, value);
            Console.WriteLine($"Added key '{key}' to the cache.");
        }

        public static bool TryAdd(IMemoryCache cache, string key, object value)
        {
            if (cache.TryGetValue(key, out _))
            {
                return false; // Already exists
            }

            cache.Set(key, value);
            return true;
        }
    }
}
