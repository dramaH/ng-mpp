using System;

namespace ARchGLCloud.Domain.Core.Events
{
    public class TenantEvent : Event
    {
        public TenantEvent(Guid id, Guid tenantId, string eName, string name, string host, string connectionString, bool enabled)
        {
            this.Id = id;
            this.TenantId = this.AggregateId = tenantId;
            this.EName = eName;
            this.Name = name;
            this.Host = host;
            this.ConnectionString = connectionString;
            this.Enabled = enabled;
        }

        public Guid Id { get; protected set; }
        public Guid TenantId { get; set; }
        public string EName { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public string ConnectionString { get; set; }
        public bool Enabled { get; set; }
    }
}
