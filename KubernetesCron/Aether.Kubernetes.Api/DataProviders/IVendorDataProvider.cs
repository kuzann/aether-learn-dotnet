using Aether.Kubernetes.Api.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aether.Kubernetes.Api.DataProviders
{
    public interface IVendorDataProvider
    {
        Task<List<VendorEntity>> List();
    }
}
