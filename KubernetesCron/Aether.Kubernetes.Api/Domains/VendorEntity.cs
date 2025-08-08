using System;

namespace Aether.Kubernetes.Api.Domains
{
    public class VendorEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactInformation { get; set; }
        public string Website { get; set; }
    }
}
