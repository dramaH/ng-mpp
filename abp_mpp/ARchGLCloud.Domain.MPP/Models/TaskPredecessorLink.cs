using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.MPP.Models
{
    /// <summary>
    ///   Defines the predecessor task on which this task depends for
    ///   its start or finish date.
    ///
    ///   Minimum: 0
    ///   Maximum: Unbounded
    ///
    ///   Refer: https://docs.microsoft.com/en-us/office-project/xml-data-interchange/xml-schema-for-the-tasks-element?view=project-client-2016
    /// </summary>
    [Table("TaskPredecessorLinks", Schema = "mpp")]
    public class TaskPredecessorLink : MppAggregateRoot<Guid>
    {
        public TaskPredecessorLink(): base(Guid.NewGuid()) { }
        public TaskPredecessorLink(Guid id) : base(id) { }

        // Unique ID number for the predecessor task.
        public int PredecessorUID { get; set; }

        // Type of predecessor task link (FF, FS, SF, SS).
        // Values are 0=FF, 1=FS, 2=SF and 3=SS
        public int Type { get; set; }

        // Indicates whether the task predecessor is part of another
        // project. default: N/A
        public bool CrossProject { get; set; }

        // External predecessor project. maxLength: N/A
        public string CrossProjectName { get; set; }

        // Amount of lag time (in tenths of a minute) from the
        // predecessor task.
        public int LinkLag { get; set; }

        // Indicates the time format (hours, days, and so on) for the
        // amount of lag specified in LinkLag.
        // Values are: 3=m, 4=em, 5=h, 6=eh, 7=d, 8=ed, 9=w, 10=ew,
        // 11=mo, 12=emo, 19=%, 20=e%, 35=m?, 36=em?, 37=h?, 38=eh?,
        // 39=d?, 40=ed?, 41=w?, 42=ew?, 43=mo?, 44=emo?, 51=%? and
        // 52=e%?
        public int LagFormat { get; set; }
    }
}