using ARchGLCloud.Domain.Core.Events;
using ARchGLCloud.Domain.MPP.Models;
using System;
using System.Collections.Generic;

namespace ARchGLCloud.Domain.MPP.Events
{
    public class TaskEvent : Event
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int UID { get; set; }

        public int _ID { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public bool IsNull { get; set; }

        public DateTime? CreateDate { get; set; }

        public string Contact { get; set; }

        public string WBS { get; set; }

        public string WBSLevel { get; set; }

        public string OutlineNumber { get; set; }

        public int OutlineLevel { get; set; }

        public int Priority { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? Finish { get; set; }

        public string Duration { get; set; }

        public int DurationFormat { get; set; }

        public string Work { get; set; }

        public DateTime? Stop { get; set; }

        public DateTime? Resume { get; set; }

        public bool ResumeValid { get; set; }

        public bool EffortDriven { get; set; }

        public bool Recurring { get; set; }

        public bool OverAllocated { get; set; }

        public bool Estimated { get; set; }

        public bool Milestone { get; set; }

        public bool Summary { get; set; }

        public bool Critical { get; set; }

        public bool IsSubproject { get; set; }

        public bool IsSubprojectReadOnly { get; set; }

        public string SubprojectName { get; set; }

        public bool ExternalTask { get; set; }

        public string ExternalTaskProject { get; set; }

        public DateTime? EarlyStart { get; set; }

        public DateTime? EarlyFinish { get; set; }

        public DateTime? LateStart { get; set; }

        public DateTime? LateFinish { get; set; }

        public int StarVariance { get; set; }

        public int FinishVariance { get; set; }

        public float WorkVariance { get; set; }

        public int FreeSlack { get; set; }

        public int TotalSlack { get; set; }

        public float FixedCost { get; set; }

        public string FixedCostAccrual { get; set; }

        public int PercentComplete { get; set; }

        public int PercentWorkComplete { get; set; }

        public decimal Cost { get; set; }

        public decimal OvertimeCost { get; set; }

        public string OvertimeWork { get; set; }

        public DateTime? ActualStart { get; set; }

        public DateTime? ActualFinish { get; set; }

        public string ActualDuration { get; set; }

        public decimal ActualCost { get; set; }

        public decimal ActualOvertimeCost { get; set; }

        public string ActualWork { get; set; }

        public string ActualOvertimeWork { get; set; }

        public string RegularWork { get; set; }

        public string RemainingDuration { get; set; }

        public decimal RemainingCost { get; set; }

        public string RemainingWork { get; set; }

        public decimal RemainingOvertimeCost { get; set; }

        public string RemainingOvertimeWork { get; set; }

        public float ACWP { get; set; }

        public float CV { get; set; }

        public int ConstraintType { get; set; }

        public int CalendarUID { get; set; }

        public DateTime? ConstraintDate { get; set; }

        public DateTime? Deadline { get; set; }

        public bool LevelAssignments { get; set; }

        public bool LevelingCanSplit { get; set; }

        public int LevelingDelay { get; set; }

        public int LevelingDelayFormat { get; set; }

        public DateTime? PreLeveledStart { get; set; }

        public DateTime? PreLeveledFinish { get; set; }

        public string Hyperlink { get; set; }

        public string HyperlinkAddress { get; set; }

        public string HyperlinkSubAddress { get; set; }

        public bool IgnoreResourceCalendar { get; set; }

        public string Notes { get; set; }

        public bool HideBar { get; set; }

        public bool Rollup { get; set; }

        public float BCWS { get; set; }

        public float BCWP { get; set; }

        public int PhysicalPercentComplete { get; set; }

        public int EarnedValueMethod { get; set; }

        public List<TaskPredecessorLink> PredecessorLink { get; set; }

        public string PredecessorLinkUUIDs { get; set; }

        public string ActualWorkProtected { get; set; }

        public string ActualOvertimeWorkProtected { get; set; }

        public List<TaskExtendedAttribute> ExtendedAttribute { get; set; }

        public string ExtendedAttributeUUIDs { get; set; }

        public List<TaskBaseline> Baseline { get; set; }

        public string BaselineUUIDs { get; set; }

        public bool IsPublished { get; set; }

        public string Statusmanager { get; set; }

        public DateTime? CommitmentStart { get; set; }

        public DateTime? CommitmentFinish { get; set; }

        public int CommitmentType { get; set; }

        public int Active { get; set; }

        public int Manual { get; set; }
    }
}
