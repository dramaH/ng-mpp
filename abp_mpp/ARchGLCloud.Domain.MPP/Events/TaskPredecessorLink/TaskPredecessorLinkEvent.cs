using ARchGLCloud.Domain.Core.Events;
using System;

namespace ARchGLCloud.Domain.MPP.Events
{
    public class TaskPredecessorLinkEvent : Event
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int PredecessorUID { get; set; }

        public int Type { get; set; }

        public bool CrossProject { get; set; }

        public string CrossProjectName { get; set; }

        public int LinkLag { get; set; }

        public int LagFormat { get; set; }
    }
}
