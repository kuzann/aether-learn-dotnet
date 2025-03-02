using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Aether.Files.Api.Services
{
    public interface IUploadService
    {
        Task ProcessExcel(IFormFile file);
    }

    public class UploadService : IUploadService
    {
        private readonly ILogger<UploadService> _logger;

        public UploadService(ILogger<UploadService> logger)
        {
            _logger = logger;
        }

        public async Task ProcessExcel(IFormFile file)
        {
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);

                    using (var reader = ExcelReaderFactory.CreateReader(file.OpenReadStream()))
                    {
                        var result = reader.AsDataSet();

                        var kitAuditTable = result.Tables["Data Master A"];
                        var auditorTable = result.Tables["Data Master B"];

                        foreach (DataRow row in kitAuditTable.Rows)
                        {
                            List<string> objects = new();
                            foreach (var item in row.ItemArray)
                            {
                                objects.Add(item?.ToString());
                            }
                            _logger.LogInformation(string.Join(",", objects));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
