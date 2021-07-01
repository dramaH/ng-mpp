using ARchGLCloud.Domain.Core.Commands;
using System;

namespace ARchGLCloud.Domain.MPP.Commands
{
    public abstract class TaskExtendedAttributeCommand : Command
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public string FieldID { get; set; }

        public string Value { get; set; }

        public string ValueGUID { get; set; }

        public int DurationFormat { get; set; }
    }
}
