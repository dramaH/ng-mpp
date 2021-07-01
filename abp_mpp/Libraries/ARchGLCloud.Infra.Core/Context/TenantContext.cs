using ARchGLCloud.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ARchGLCloud.Infra.Core.Context
{
    public class TenantContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public IConfiguration Configuration { get; }
        public TenantContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("project");
            modelBuilder.Entity<Tenant>().HasKey(e => e.Id);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // define the database to use
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("TenantConnection"));

            base.OnConfiguring(optionsBuilder);
        }
    }
}
