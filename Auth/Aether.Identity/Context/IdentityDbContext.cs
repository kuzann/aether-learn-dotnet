using Aether.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aether.Identity.Context
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> User { get; set; }
    }
}
