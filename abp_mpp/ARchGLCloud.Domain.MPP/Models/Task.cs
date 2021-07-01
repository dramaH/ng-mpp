using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.MPP.Models
{
    /// <summary>
    ///   The collection of tasks that make up the project.
    ///   There must be at least one task in each Tasks collection.
    ///
    ///   See: https://docs.microsoft.com/en-us/office-project/xml-data-interchange/task-elements-and-xml-structure?view=project-client-2016
    /// </summary>
    [Table("Tasks", Schema = "mpp")]
    public class Task : MppAggregateRoot<Guid>
    {
        public Task() : base(Guid.NewGuid())
        {
            if (PredecessorLink == null) PredecessorLink = new List<TaskPredecessorLink>();
            ExtendedAttribute = new List<TaskExtendedAttribute>();
            Baseline = new List<TaskBaseline>();
        }

        public Task(Guid id) : base(id)
        {
            if (PredecessorLink == null) PredecessorLink = new List<TaskPredecessorLink>();
            ExtendedAttribute = new List<TaskExtendedAttribute>();
            Baseline = new List<TaskBaseline>();
        }

        // The unique ID of the task.
        public int UID { get; set; }

        // The position identifier of the task within the list of
        // tasks. Duplicate column name 'ID' (Id already exists on
        // MppAggregateRoot)
        public int _ID { get; set; }

        // The name of the task. maxLength: 512
        public string Name { get; set; }

        // The type of task. Values are: 0=Fixed Units, 1=Fixed
        // Duration, 2=Fixed Work.
        public int Type { get; set; }

        // Specifies whether the task is null. default: N/A
        public bool IsNull { get; set; }

        // The date that the task was created.
        public DateTime? CreateDate { get; set; }

        // The contact person for the task. maxLength: 512
        public string Contact { get; set; }

        // The work breakdown structure (WBS) code of the task. maxLength: N/A
        public string WBS { get; set; }

        // The right-most WBS level of the task. For example, if the
        // task level was A.01.03, the right-most level would be
        // 03. maxLength: N/A
        public string WBSLevel { get; set; }

        // The outline number of the task. maxLength = 512.
        public string OutlineNumber { get; set; }

        // The outline level of the task.
        public int OutlineLevel { get; set; }

        // he priority of the task from 0 to 1000.
        public int Priority { get; set; }

        // The scheduled start date of the task.
        public DateTime? Start { get; set; }

        // The scheduled finish date of the task.
        public DateTime? Finish { get; set; }

        // Duration, The planned duration of the task.
        public string Duration { get; set; }

        // The format for expressing the Duration of the Task.  Values
        // are: 3=m, 4=em, 5=h, 6=eh, 7=d, 8=ed, 9=w, 10=ew, 11=mo,
        // 12=emo, 19=%, 20=e%, 21=null, 35=m?, 36=em?, 37=h?, 38=eh?,
        // 39=d?, 40=ed?, 41=w?, 42=ew?, 43=mo?, 44=emo?, 51=%?,
        // 52=e%? and 53=null.
        public int DurationFormat { get; set; }

        // Duration, The amount of scheduled work for the task.
        public string Work { get; set; }

        // The date that the task was stopped.
        public DateTime? Stop { get; set; }

        // The date that the task resumed.
        public DateTime? Resume { get; set; }

        // Whether the task can be resumed. default: N/A
        public bool ResumeValid { get; set; }

        // Whether the task is effort-driven. default: N/A
        public bool EffortDriven { get; set; }

        // Whether the task is a recurring task. default: N/A
        public bool Recurring { get; set; }

        // Whether the task is overallocated. This element is for
        // information only. default: N/A
        public bool OverAllocated { get; set; }

        // Whether the task is estimated. default: N/A
        public bool Estimated { get; set; }

        // Whether the task is a milestone. default: N/A
        public bool Milestone { get; set; }

        // Whether the task is a summary task. default: N/A
        public bool Summary { get; set; }

        // Whether the task is in the critical chain. default: N/A
        public bool Critical { get; set; }

        // Whether the task is an inserted project. default: N/A
        public bool IsSubproject { get; set; }

        // Whether the inserted project is read-only. default: N/A
        public bool IsSubprojectReadOnly { get; set; }

        // The source location of the inserted project. maxLength: 512
        public string SubprojectName { get; set; }

        // Whether the task is external. default: N/A
        public bool ExternalTask { get; set; }

        // The source location and task identifier of the external
        // task. maxLength: 512
        public string ExternalTaskProject { get; set; }

        // The early start date of the task.
        public DateTime? EarlyStart { get; set; }

        // The early finish date of the task.
        public DateTime? EarlyFinish { get; set; }

        // The late start date of the task.
        public DateTime? LateStart { get; set; }

        // The late finish date of the task.
        public DateTime? LateFinish { get; set; }

        // The variance of the task start date from the baseline start
        // date as minutes x 1000.
        public int StarVariance { get; set; }

        // The variance of the task finish date from the baseline
        // finish date as minutes x 1000.
        public int FinishVariance { get; set; }

        // The variance of task work from the baseline task work as
        // minutes x 1000.
        public float WorkVariance { get; set; }

        // The amount of free slack.
        public int FreeSlack { get; set; }

        // The amount of total slack.
        public int TotalSlack { get; set; }

        // The fixed cost of the task.
        public float FixedCost { get; set; }

        // How the fixed cost is accrued against the task. Values are:
        // 1=Start, 2=Prorated and 3=End.
        public string FixedCostAccrual { get; set; }

        // The percentage of the task duration completed.
        public int PercentComplete { get; set; }

        // The percentage of the task work completed.
        public int PercentWorkComplete { get; set; }

        // The projected or scheduled cost of the task.
        public decimal Cost { get; set; }

        // The sum of the actual and remaining overtime cost of the
        // task.
        public decimal OvertimeCost { get; set; }

        // Duration, The amount of overtime work scheduled for the task.
        public string OvertimeWork { get; set; }

        // The actual start date of the task.
        public DateTime? ActualStart { get; set; }

        // The actual finish date of the task.
        public DateTime? ActualFinish { get; set; }

        // Duration, The actual duration of the task.
        public string ActualDuration { get; set; }

        // The actual cost of the task.
        public decimal ActualCost { get; set; }

        // The actual overtime cost of the task.
        public decimal ActualOvertimeCost { get; set; }

        // Duration, The actual work for the task.
        public string ActualWork { get; set; }

        // Duration, The actual overtime work for the task.
        public string ActualOvertimeWork { get; set; }

        // Duration, The amount of non-overtime work scheduled for the task.
        public string RegularWork { get; set; }

        // Duration, The amount of time required to complete the
        // unfinished portion of the task.
        public string RemainingDuration { get; set; }

        // The remaining projected cost of completing the task.
        public decimal RemainingCost { get; set; }

        // Duration, The remaining work scheduled to complete the task.
        public string RemainingWork { get; set; }

        // The remaining overtime cost projected to finish the task.
        public decimal RemainingOvertimeCost { get; set; }

        // Duration, The remaining overtime work scheduled to finish the task.
        public string RemainingOvertimeWork { get; set; }

        // The actual cost of work performed on the task to date.
        public float ACWP { get; set; }

        // Earned value cost variance.
        public float CV { get; set; }

        // The constraint on the start or finish date of the
        // task. Values are: 0=As Soon As Possible, 1=As Late As
        // Possible, 2=Must Start On, 3=Must Finish On, 4=Start No
        // Earlier Than, 5=Start No Later Than, 6=Finish No Earlier
        // Than and 7=Finish No Later Than.
        public int ConstraintType { get; set; }

        // The task calendar. Refers to a valid UID in the Calendars
        // element of the Microsoft Office Project 2007 XML Schema.
        public int CalendarUID { get; set; }

        // The date argument for the task constraint type.
        public DateTime? ConstraintDate { get; set; }

        // The deadline for the task to be completed.
        public DateTime? Deadline { get; set; }

        // Whether leveling can adjust assignments. default: N/A
        public bool LevelAssignments { get; set; }

        // Whether leveling can split the task. default: N/A
        public bool LevelingCanSplit { get; set; }

        // The delay caused by leveling the task.
        public int LevelingDelay { get; set; }

        // The format for expressing the duration of the delay.
        // Values are: 3=m, 4=em, 5=h, 6=eh, 7=d, 8=ed, 9=w, 10=ew,
        // 11=mo, 12=emo, 19=%, 20=e%, 21=null, 35=m?, 36=em?, 37=h?,
        // 38=eh?, 39=d?, 40=ed?, 41=w?, 42=ew?, 43=mo?, 44=emo?,
        // 51=%?, 52=e%? and 53=null.
        public int LevelingDelayFormat { get; set; }

        // The start date of the task before it was leveled.
        public DateTime? PreLeveledStart { get; set; }

        // The finish date of the task before it was leveled.
        public DateTime? PreLeveledFinish { get; set; }

        // The title of the hyperlink associated with the
        // task. maxLength: 512
        public string Hyperlink { get; set; }

        // The hyperlink associated with the task. maxLength: 512
        public string HyperlinkAddress { get; set; }

        // The document bookmark of the hyperlink associated with the
        // task. maxLength: 512
        public string HyperlinkSubAddress { get; set; }

        // Whether the task ignores the resource calendar. default: N/A
        public bool IgnoreResourceCalendar { get; set; }

        // Text notes associated with the task. maxLength: N/A
        public string Notes { get; set; }

        // Whether the GANTT bar of the task is hidden when displayed
        // in Microsoft Office Project. default: N/A
        public bool HideBar { get; set; }

        // Whether the task is rolled up. default: N/A
        public bool Rollup { get; set; }

        // The budgeted cost of work scheduled for the task.
        public float BCWS { get; set; }

        // The budgeted cost of work performed on the task to date.
        public float BCWP { get; set; }

        // The percentage complete value entered by the Project
        // Manager.  This can be used as an alternative for
        // calculating the budgeted cost of work performed (BCWP).
        public int PhysicalPercentComplete { get; set; }

        // The method for calculating earned value. Values are:
        // 0=Percent Complete, 1=Physical Percent Complete.
        public int EarnedValueMethod { get; set; }

        // <xsd:element name="PredecessorLink" minOccurs="0" maxOccurs="unbounded">
        public virtual List<TaskPredecessorLink> PredecessorLink { get; set; }
        public string PredecessorLinkUUIDs { get; set; }

        // Duration, Specifies the duration through which actual work
        // is protected.
        public string ActualWorkProtected { get; set; }

        // Duration, Specifies the duration through which actual
        // overtime work is protected.
        public string ActualOvertimeWorkProtected { get; set; }

        // <xsd:element name="ExtendedAttribute" minOccurs="0" maxOccurs="unbounded">
        public virtual List<TaskExtendedAttribute> ExtendedAttribute { get; set; }
        public string ExtendedAttributeUUIDs { get; set; }

        // <xsd:element name="Baseline" minOccurs="0" maxOccurs="unbounded">
        public virtual List<TaskBaseline> Baseline { get; set; }
        public string BaselineUUIDs { get; set; }

        // FIXME: <xsd:element name="OutlineCode" minOccurs="0" maxOccurs="unbounded">

        // Specifies whether the task is published. default: N/A
        public bool IsPublished { get; set; }

        // The name of the task status manager. maxLength: N/A
        public string Statusmanager { get; set; }

        // The start date of the deliverable.
        public DateTime? CommitmentStart { get; set; }

        // The finish date of the deliverable.
        public DateTime? CommitmentFinish { get; set; }

        // Specifies whether the task has an associated deliverable or
        // a dependency on an associated deliverable. Values are:
        // 0=The task has no deliverable or dependency on a
        // deliverable, 1=The task has an associated deliverable,
        // 2=The task has a dependency on an associated deliverable.
        public int CommitmentType { get; set; }

        // FIXME: <xsd:element name="TimephasedData" type="TimephasedDataType" minOccurs="0" maxOccurs="unbounded">

        // NOT USED, but SHOULD exist for backward-compatibility
        public int Active { get; set; }
        public int Manual { get; set; }

        // 自定义属性
        public string CustomAttrs { get; set; }

        /// <summary>是否里程碑</summary>
        public bool IsMilepost { get; set; }
    }
}
