using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.MPP.Models
{
    /// <summary>
    ///   The collection of assignments that make up the project.
    ///   There must be at least one assignment in each Assignments collection.
    /// </summary>
    [Table("Assignments", Schema = "mpp")]
    public class Assignment: MppAggregateRoot<Guid>
    {
        public Assignment(): base(Guid.NewGuid()) { }
        public Assignment(Guid id): base(id) { }

        // The unique identifier of the assignment.
        public int UID { get; set; }

        // The unique identifier of the task.
        public int TaskUID { get; set; }

        // The unique identifier of the resource.
        public int ResourceUID { get; set; }

        // The amount of work completed on the assignment.
        public int PercentWorkComplete { get; set; }

        // The actual cost incurred on the assignment.
        public decimal ActualCost { get; set; }

        // The actual finish date of the assignment.
        public DateTime? ActualFinish { get; set; }

        // The actual overtime cost incurred on the assignment.
        public decimal ActualOvertimeCost { get; set; }

        // The actual amount of overtime work incurred on the
        // assignment.
        public string ActualOvertimeWork { get; set; }

        // The actual start date of the assignment.
        public DateTime? AcutalStart { get; set; }

        // The actual amount of work incurred on the assignment.
        public string ActualWork { get; set; }

        // The actual cost of work performed on the assignment to
        // date.
        public float ACWP { get; set; }

        // Whether the Resource has accepted all of his or her
        // assignments.
        public bool Confirmed { get; set; }

        // The projected or scheduled cost of the assignment.
        public decimal Cost { get; set; }

        // The cost rate table used for the assignment.
        public int CostRateTalbe { get; set; }

        // The difference between the cost and baseline cost for a
        // resource.
        public float CostVariance { get; set; }

        // Earned value cost variance.
        public float CV { get; set; }

        // The amount that the assignment is delayed.
        public int Delay { get; set; }

        // The scheduled finish date of the assignment.
        public DateTime? Finish { get; set; }

        // The variance of the assignment finish date from the
        // baseline finish date.
        public int FinishVariance { get; set; }

        // The title of the hyperlink associated with the assignment.
        [MaxLength(512)]
        public string Hyperlink { get; set; }

        // The hyperlink associated with the assignment.
        [MaxLength(512)]
        public string HyperlinkAddress { get; set; }

        // The document bookmark of the hyperlink associated with the
        // assignment.
        [MaxLength(512)]
        public string HyperlinkSubAddress { get; set; }

        // The variance of assignment work from the baseline work as
        // minutes x 1000.
        public float WorkVariance { get; set; }

        // Whether the Units are Fixed Rate.
        public bool HasFixedRateUnits { get; set; }

        // Whether the consumption of the assigned material resource
        // occurs in a single, fixed amount.
        public bool FixedMaterial { get; set; }

        // The delay caused by leveling.
        public int LevelingDelay { get; set; }

        // The format for expressing the duration of the delay.
        // Values are: 3=m, 4=em, 5=h, 6=eh, 7=d, 8=ed, 9=w, 10=ew,
        // 11=mo, 12=emo, 19=%, 20=e%, 21=null, 35=m?, 36=em?, 37=h?,
        // 38=eh?, 39=d?, 40=ed?, 41=w?, 42=ew?, 43=mo?, 44=emo?,
        // 51=%?, 52=e%? and 53=null.
        public int LevelingDelayFormat { get; set; }

        // Whether the project is linked to another OLE object.
        public bool LinkedFields { get; set; }

        // Whether the assignment is a milestone.
        public bool Milestone { get; set; }

        // Text notes associated with the assignment.
        public string Notes { get; set; }

        // Whether the assignment is overallocated.
        public bool Overallocated { get; set; }

        // The sum of the actual and remaining overtime cost of the
        // assignment.
        public decimal OvertimeCost { get; set; }

        // The scheduled overtime work scheduled for the assignment.
        public string OvertimeWork { get; set; }

        // The largest number of units that a resource is assigned for
        // a task.
        public float PeakUnits { get; set; }

        // The amount of non-overtime work scheduled for the
        // assignment.
        public string RegularWork { get; set; }

        // The remaining projected cost of completing the assignment.
        public decimal RemainingCost { get; set; }

        // The remaining projected overtime cost of completing the
        // assignment.
        public decimal RemainingOvertimeCost { get; set; }

        // The remaining overtime work scheduled to complete the
        // assignment.
        public string RemainingOvertimeWork { get; set; }

        // The remaining work scheduled to complete the assignment.
        public string RemainingWork { get; set; }

        // True if a response has not been received for a TeamAssign
        // message.
        public bool ResponsePending { get; set; }

        // The scheduled start date of the assignment.
        public DateTime? Start { get; set; }

        // The date that the assignment was stopped.
        public DateTime? Stop { get; set; }

        // The date that the assignment resumed.
        public DateTime? Resume { get; set; }

        // The variance of the assignment start date from the baseline
        // start date.
        public int StartVariance { get; set; }

        // Specifies whether the task is a summary task.
        public bool Summary { get; set; }

        // Earned value schedule variance, through the project status
        // date.
        public float SV { get; set; }

        // The number of units for the assignment.
        public float Units { get; set; }

        // True if the resource assigned to a task needs to be updated
        // as to the status of the task.
        public bool UpadteNeeded { get; set; }

        // The difference between basline cost and total cost.
        public float VAC { get; set; }

        // The amount of scheduled work for the assignment.
        public string Work { get; set; }

        // The work contour of the assignment. Values are: 0=Flat,
        // 1=Back Loaded, 2=Front Loaded, 3=Double Peak, 4=Early Peak,
        // 5=Late Peak, 6=Bell, 7=Turtle, 8=Contoured.
        public int WorkContour { get; set; }

        // The budgeted cost of work on the assignment.
        public float BCWS { get; set; }

        // The budgeted cost of work performed on the assignment to
        // date.
        public float BCWP { get; set; }

        // Specifies the booking type of the assignment. 1=Commited,
        // 2=Proposed.
        public int BookingType { get; set; }

        // Specifies the duration through which actual work is
        // protected.
        public string ActualWorkProtected { get; set; }

        // Specifies the duration through which actual overtime work
        // is protected.
        public string ActualOvertimeWorkProtected { get; set; }

        // The date that the assignment was created.
        public DateTime? CreationDate { get; set; }

        // The name of the assignment owner.
        public string AssnOwner { get; set; }

        // The GUID of the assignment owner.
        public string AssnOwnerGuid { get; set; }

        // The budgeted amount for cost resources on this assignment.
        public decimal BudgetCost { get; set; }

        // The budgeted work amount for work or material resources on
        // this assignment.
        public string BudgetWork { get; set; }

        // <xsd:element name="ExtendedAttribute" minOccurs="0" maxOccurs="unbounded">
        // <xsd:element name="Baseline" minOccurs="0" maxOccurs="unbounded">
        // <xsd:element name="TimephasedData" type="TimephasedDataType" minOccurs="0" maxOccurs="unbounded">
    }
}