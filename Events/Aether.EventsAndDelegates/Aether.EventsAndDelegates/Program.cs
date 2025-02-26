namespace Aether.EventsAndDelegates
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var video = new Video()
            {
                Title = "Video 1"
            };
            var videoEncoder = new VideoEncoder(); // publisher
            var mailService = new MailService(); // subscriber
            var messageService = new MessageService(); // subscriber

            videoEncoder.VideoEncoded += mailService.OnVideoEncoded; // subscribe
            videoEncoder.VideoEncoded += messageService.OnVideoEncoded; // subscribe

            videoEncoder.Encode(video);

            Console.WriteLine("End of program");
        }
    }
}