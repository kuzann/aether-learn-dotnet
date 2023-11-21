using Aether.SampleApplication.Model.Request;

namespace Aether.SampleApplication.Entities
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }

        public Song()
        {
            
        }

        // you shouldn't map request directly into entity, use auto mapper or clean architecture instead for real application
        public Song(SongRequest request)
        {
            Name = request.Name;
            Artist = request.Artist;
            Genre = request.Genre;
            Year = request.Year;
        }
    }
}
