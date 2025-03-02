using Microsoft.AspNetCore.Http;

namespace Aether.Files.Api.Model
{
    public class UploadRequest
    {
        public IFormFile File { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
