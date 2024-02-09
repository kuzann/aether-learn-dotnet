namespace Aether.EventsAndDelegates
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var video = new Video()
            //{
            //    Title = "Video 1"
            //};
            //var videoEncoder = new VideoEncoder(); // publisher
            //var mailService = new MailService(); // subscriber
            //var messageService = new MessageService(); // subscriber

            //videoEncoder.VideoEncoded += mailService.OnVideoEncoded; // subscribe
            //videoEncoder.VideoEncoded += messageService.OnVideoEncoded; // subscribe

            //videoEncoder.Encode(video);

            Console.WriteLine(NumberOfWays(842, 91)); //143119619
        }

        public static int NumberOfWays(int N, int K)
        {
            // Base case
            if (N == 0)
                return 1;
            if (N < 0 || K <= 0)
                return 0;

            // including and not including K in sum
            return NumberOfWays(N - K, K) + NumberOfWays(N, K - 1);
        }
    }
}