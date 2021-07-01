using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.MPP.Models
{
    /// <summary>
    ///   The collection of baseline values of the task.
    /// </summary>
    [Table("TaskBaselines", Schema = "mpp")]
    public class TaskBaseline : MppAggregateRoot<Guid>
    {
        public TaskBaseline(): base(Guid.NewGuid()) { }
        public TaskBaseline(Guid id): base(id) { }

        // FIXME: <xsd:element name="TimephasedData" type="TimephasedDataType" minOccurs="0" maxOccurs="unbounded">

        // The unique number of the baseline data record.
        public int Number { get; set; }

        // Whether this is an interim baseline. default: false
        public bool Interim { get; set; }

        // The scheduled start date of the task when the baseline was
        // saved.
        public DateTime? Start { get; set; }

        // The scheduled finish date of the task when the baseline was
        // saved.
        public DateTime? Finish { get; set; }

        // Duration, The scheduled duration of the task when the
        // baseline was saved.
        public string Duration { get; set; }

        // The format for expressing the Duration of the Task
        // baseline.  Values are: 3=m, 4=em, 5=h, 6=eh, 7=d, 8=ed,
        // 9=w, 10=ew, 11=mo, 12=emo, 19=%, 20=e%, 21=null, 35=m?,
        // 36=em?, 37=h?, 38=eh?, 39=d?, 40=ed?, 41=w?, 42=ew?,
        // 43=mo?, 44=emo?, 51=%?, 52=e%? and 53=null.
        public int DurationFormat { get; set; }

        // Whether the baseline duration of the task was
        // estimated. default: N/A
        public bool EstimatedDuration { get; set; }

        // Duration, The scheduled work of the task when the baseline
        // was saved.
        public string Work { get; set; }

        // The projected cost of the task when the baseline was saved.
        public decimal Cost { get; set; }

        // The budgeted cost of work scheduled for the task.
        public float BCWS { get; set; }

        // The budgeted cost of work performed on the task to date.
        public float BCWP { get; set; }

        // The fixed cost of the task when the baseline was saved.
        public float FixedCost { get; set; }
    }
}