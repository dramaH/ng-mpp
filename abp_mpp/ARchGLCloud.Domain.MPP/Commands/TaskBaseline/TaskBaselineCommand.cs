using ARchGLCloud.Domain.Core.Commands;
using System;

namespace ARchGLCloud.Domain.MPP.Commands
{
    public abstract class TaskBaselineCommand : Command
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int Number { get; set; }

        public bool Interim { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? Finish { get; set; }

        public string Duration { get; set; }

        public int DurationFormat { get; set; }

        public bool EstimatedDuration { get; set; }

        public string Work { get; set; }

        public decimal Cost { get; set; }

        public float BCWS { get; set; }

        public float BCWP { get; set; }

        public float FixedCost { get; set; }
    }
}
