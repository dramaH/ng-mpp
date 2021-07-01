using ARchGLCloud.Domain.Core.Events;
using System;

namespace ARchGLCloud.Domain.MPP.Events
{
    public class ExtendedAttributeRemovedEvent : Event
    {
        public ExtendedAttributeRemovedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
