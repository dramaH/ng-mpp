using ARchGLCloud.Domain.Core.Events;
using System;

namespace ARchGLCloud.Domain.MPP.Events
{
    public class ResourceEvent : Event
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int UID { get; set; }

        public int _ID { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public bool IsNull { get; set; }

        public string Initials { get; set; }

        public string Phonetics { get; set; }

        public string NTAccount { get; set; }

        public string MaterialLabel { get; set; }

        public string Code { get; set; }

        public string Group { get; set; }

        public int WorkGroup { get; set; }

        public string EmailAddress { get; set; }

        public string Hyperlink { get; set; }

        public string HyperlinkAddress { get; set; }

        public string HyperlinkSubAddress { get; set; }

        public float MaxUnits { get; set; }

        public float PeakUnits { get; set; }

        public bool OverAllocated { get; set; }

        public DateTime? AvailableFrom { get; set; }

        public DateTime? AvailableTo { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? Finish { get; set; }

        public bool CanLevel { get; set; }

        public int AccrueAt { get; set; }

        public string Work { get; set; }

        public string RegularWork { get; set; }

        public string OvertimeWork { get; set; }

        public string ActualWork { get; set; }

        public string RemainingWork { get; set; }

        public string ActualOvertimeWork { get; set; }

        public string RemainingOvertimeWork { get; set; }

        public int PercentWorkComplete { get; set; }

        public decimal StandardRate { get; set; }

        public int StandardRateFormat { get; set; }

        public decimal Cost { get; set; }

        public decimal OvertimeRate { get; set; }

        public int OvertimeRateFormat { get; set; }

        public decimal OvertimeCost { get; set; }

        public decimal CostPerUse { get; set; }

        public decimal ActualCost { get; set; }

        public decimal ActualOvertimeCost { get; set; }

        public decimal RemainingCost { get; set; }

        public decimal RemainingOvertimeCost { get; set; }

        public float WorkVariance { get; set; }

        public float CostVariance { get; set; }

        public float SV { get; set; }

        public float CV { get; set; }

        public float ACWP { get; set; }

        public int CalendarUID { get; set; }

        public string Notes { get; set; }

        public float BCWS { get; set; }

        public float BCWP { get; set; }

        public bool IsGeneric { get; set; }

        public bool IsInactive { get; set; }

        public bool IsEnterprise { get; set; }

        public int BookingType { get; set; }

        public string ActualWorkProtected { get; set; }

        public string ActualOvertimeWorkProtected { get; set; }

        public string ActiveDirectoryGUID { get; set; }

        public DateTime? CreationDate { get; set; }

        public bool IsCostResource { get; set; }

        public string AssnOwner { get; set; }

        public string AssnOwnerGuid { get; set; }

        public bool IsBudget { get; set; }
    }
}
