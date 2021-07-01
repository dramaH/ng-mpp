using ARchGLCloud.Domain.Core.Events;
using System;

namespace ARchGLCloud.Domain.MPP.Events
{
    public class TaskExtendedAttributeEvent : Event
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public string FieldID { get; set; }

        public string Value { get; set; }

        public string ValueGUID { get; set; }

        public int DurationFormat { get; set; }
    }
}
