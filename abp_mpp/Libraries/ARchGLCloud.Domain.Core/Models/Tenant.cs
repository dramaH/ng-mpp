using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.Core.Models
{
    [Table("Tenants", Schema = "project")]
    public class Tenant : AggregateRoot<Guid>
    {
        public Tenant()
        {
        }

        public Tenant(Guid id, Guid tenantId) : base(id)
        {
            this.TenantId = tenantId;
        }

        public Guid TenantId { get; set; }
        public string EName { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public string ConnectionString { get; set; }
        public bool Enabled { get; set; }
    }
}
