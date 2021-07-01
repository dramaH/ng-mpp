using ARchGLCloud.Domain.Core.Events;
using System;

namespace ARchGLCloud.Domain.MPP.Events
{
    public class ProjectRemovedEvent : Event
    {
        public ProjectRemovedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
