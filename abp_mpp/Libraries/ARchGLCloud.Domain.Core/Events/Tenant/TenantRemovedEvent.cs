using System;

namespace ARchGLCloud.Domain.Core.Events
{
    public class TenantRemovedEvent : Event
    {
        public TenantRemovedEvent(Guid id, Guid tenantId)
        {
            AggregateId = tenantId;
        }

        public Guid Id { get; set; }
    }
}
