using ARchGLCloud.Domain.Core.Events;
using System;

namespace ARchGLCloud.Domain.MPP.Events
{
    public class AssignmentEvent : Event
    {
        public Guid ParentId { get; set; }
        public int UID { get; set; }
        public int TaskUID { get; set; }
        public int ResourceUID { get; set; }
        public int PercentWorkComplete { get; set; }
        public decimal ActualCost { get; set; }
        public DateTime? ActualFinish { get; set; }
        public decimal ActualOvertimeCost { get; set; }
        public string ActualOvertimeWork { get; set; }
        public DateTime? AcutalStart { get; set; }
        public string ActualWork { get; set; }
        public float ACWP { get; set; }
        public bool Confirmed { get; set; }
        public decimal Cost { get; set; }
        public int CostRateTalbe { get; set; }
        public float CostVariance { get; set; }
        public float CV { get; set; }
        public int Delay { get; set; }
        public DateTime? Finish { get; set; }
        public int FinishVariance { get; set; }
        public string Hyperlink { get; set; }
        public string HyperlinkAddress { get; set; }
        public string HyperlinkSubAddress { get; set; }
        public float WorkVariance { get; set; }
        public bool HasFixedRateUnits { get; set; }
        public bool FixedMaterial { get; set; }
        public int LevelingDelay { get; set; }
        public int LevelingDelayFormat { get; set; }
        public bool LinkedFields { get; set; }
        public bool Milestone { get; set; }
        public string Notes { get; set; }
        public bool Overallocated { get; set; }
        public decimal OvertimeCost { get; set; }
        public string OvertimeWork { get; set; }
        public float PeakUnits { get; set; }
        public string RegularWork { get; set; }
        public decimal RemainingCost { get; set; }
        public decimal RemainingOvertimeCost { get; set; }
        public string RemainingOvertimeWork { get; set; }
        public string RemainingWork { get; set; }
        public bool ResponsePending { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Stop { get; set; }
        public DateTime? Resume { get; set; }
        public int StartVariance { get; set; }
        public bool Summary { get; set; }
        public float SV { get; set; }
        public float Units { get; set; }
        public bool UpadteNeeded { get; set; }
        public float VAC { get; set; }
        public string Work { get; set; }
        public int WorkContour { get; set; }
        public float BCWS { get; set; }
        public float BCWP { get; set; }
        public int BookingType { get; set; }
        public string ActualWorkProtected { get; set; }
        public string ActualOvertimeWorkProtected { get; set; }
        public DateTime? CreationDate { get; set; }
        public string AssnOwner { get; set; }
        public string AssnOwnerGuid { get; set; }
        public decimal BudgetCost { get; set; }
        public string BudgetWork { get; set; }
    }
}
