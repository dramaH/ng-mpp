using System;
using System.Linq;
using System.Reflection;
using ARchGLCloud.Domain.Core.Events;
using ARchGLCloud.Domain.Core.Models;
using ARchGLCloud.Domain.MPP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ARchGLCloud.Infra.MPP.Context
{
    public class MPPContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public DbSet<StoredEvent> StoredEvents { get; set; }
        public MPPContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            applyConfiguration(modelBuilder);

            var typesToRegister = Assembly.Load("ARchGLCloud.Domain.MPP").GetTypes()
                .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
                                          .Where(type => type.BaseType != null &&
                                                 type.BaseType.IsGenericType &&
                                                 type.BaseType.GetGenericTypeDefinition() == typeof(MppAggregateRoot<>));

            foreach (var type in typesToRegister)
            {
                if (modelBuilder.Model.FindEntityType(type) != null)
                    continue;

                modelBuilder.Model.AddEntityType(type);
            }


            base.OnModelCreating(modelBuilder);
        }
        private void applyConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("mpp");

            var typesToRegister = Assembly.Load("ARchGLCloud.Infra.MPP").GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType)
                .Where(type => type.BaseType.GetInterfaces().Any(i => i.Name == typeof(IEntityTypeConfiguration<>).Name));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // define the database to use
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("MPPConnection"));
        }
    }
}
