namespace Aether.EventsAndDelegates
{
    public class MailService
    {
        public void OnVideoEncoded(object source, VideoEventArgs args)
        {
            Console.WriteLine($"Mail Service sending an email for video {args.Video.Title}...");
        }
    }
}