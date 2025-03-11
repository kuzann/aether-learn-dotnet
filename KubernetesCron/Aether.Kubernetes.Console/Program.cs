namespace Aether.Kubernetes.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Cron Job started...");
            while (true)
            {
                System.Console.WriteLine($"Cron job running at {DateTime.UtcNow}");
                await Task.Delay(TimeSpan.FromMinutes(1)); // Run every 1 minute
            }
        }
    }
}
