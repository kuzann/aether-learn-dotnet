using Aether.SampleApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aether.SampleApplication.Context
{
    public class SampleAppDbContext : DbContext
    {
        public SampleAppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Song> Song { get; set; }
    }
}
