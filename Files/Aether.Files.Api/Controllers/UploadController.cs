using Aether.Files.Api.Model;
using Aether.Files.Api.Services;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Aether.Files.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] UploadRequest request)
        {
            switch (request.Type)
            {
                case "Excel":
                    await _uploadService.ProcessExcel(request.File);
                    break;
                default:
                    break;
            }


            return Ok();
        }
    }
}
