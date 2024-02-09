using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aether.EventsAndDelegates
{
    /// <summary>
    /// 1. Define a delegate (contract between publisher and subscriber)
    /// 2. Define event based on delegate
    /// 3. Raise event
    /// </summary>
    public class VideoEncoder
    {
        // The old way
        //public delegate void VideoEncodedEventHandlers(object source, VideoEventArgs args);
        //public event VideoEncodedEventHandlers VideoEncoded;

        // The simple way
        public EventHandler<VideoEventArgs> VideoEncoded; 

        public void Encode(Video video)
        {
            Console.WriteLine("Encoding Video...");
            Thread.Sleep(2000);

            OnVideoEncoded(video);
        }

        protected virtual void OnVideoEncoded(Video video)
        {
            if (VideoEncoded != null)
            {
                VideoEncoded(this, new VideoEventArgs() { Video = video });
            }
        }
    }

    public class VideoEventArgs : EventArgs
    {
        public Video Video { get; set; }
    }
}
