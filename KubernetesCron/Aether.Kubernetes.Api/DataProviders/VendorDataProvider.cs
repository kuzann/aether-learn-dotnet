using Aether.Kubernetes.Api.Domains;
using Aether.Kubernetes.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aether.Kubernetes.Api.DataProviders
{
    public class VendorDataProvider : IVendorDataProvider
    {
        private readonly ApplicationContext _context;

        public VendorDataProvider(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<VendorEntity>> List()
        {
            return await _context.Vendors.ToListAsync();
        }
    }
}
