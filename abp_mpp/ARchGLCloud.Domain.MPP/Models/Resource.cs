using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.MPP.Models
{
    /// <summary>
    ///   The collection of resources that make up the project.
    ///   There must be at least one resource in each Resources collection.
    /// </summary>
    [Table("Resources", Schema = "mpp")]
    public class Resource: MppAggregateRoot<Guid>
    {
        public Resource(): base(Guid.NewGuid()) { }
        public Resource(Guid id): base(id) { }

        // The unique identifier of the resource.
        public int UID { get; set; }

        // The position identifier of the resource within the list of resources.
        public int _ID { get; set; }

        // The name of the resource.
        [MaxLength(512)]
        public string Name { get; set; }

        // The type of resource. Values are: 0=Material, 1=Work.
        public int Type { get; set; }

        // Specifies whether the resource is null.
        public bool IsNull { get; set; }

        // The initials of the resource.
        [MaxLength(512)]
        public string Initials { get; set; }

        // The phonetic spelling of the resource name.  For use with
        // Japanese only.
        [MaxLength(512)]
        public string Phonetics { get; set; }

        // The NT account associated with the resource.
        [MaxLength(512)]
        public string NTAccount { get; set; }

        // The unit of measure for the material resource.
        [MaxLength(512)]
        public string MaterialLabel { get; set; }

        // The code or other information about the resource.
        [MaxLength(512)]
        public string Code { get; set; }

        // The group to which the resource belongs.
        [MaxLength(512)]
        public string Group { get; set; }

        // The type of workgroup to which the resource belongs. Values
        // are: 0=Default, 1=None, 2=Email, 3=Web.
        public int WorkGroup { get; set; }

        // The e-mail address of the resource.
        [MaxLength(512)]
        public string EmailAddress { get; set; }

        // The title of the hyperlink associated with the resource.
        [MaxLength(512)]
        public string Hyperlink { get; set; }

        // The hyperlink associated with the resource.
        [MaxLength(512)]
        public string HyperlinkAddress { get; set; }

        // The document bookmark of the hyperlink associated with the
        // resource.
        [MaxLength(512)]
        public string HyperlinkSubAddress { get; set; }

        // The maximum number of units that the resource is
        // available. default: 1.0
        public float MaxUnits { get; set; }

        // The largest number of units assigned to the resource at any
        // time.
        public float PeakUnits { get; set; }

        // Whether the resource is overallocated.
        public bool OverAllocated { get; set; }

        // The first date that the resource is available.
        public DateTime? AvailableFrom { get; set; }

        // The last date the resource is available.
        public DateTime? AvailableTo { get; set; }

        // The scheduled start date of the resource.
        public DateTime? Start { get; set; }

        // The scheduled finish date of the resource.
        public DateTime? Finish { get; set; }

        // Whether the resource can be leveled.
        public bool CanLevel { get; set; }

        // How cost is accrued against the resource. Values are:
        // 1=Start, 2=End, 3=Prorated, 4=Invalid.
        public int AccrueAt { get; set; }

        // Duration, The total work assigned to the resource across
        // all assigned tasks.
        public string Work { get; set; }

        // Duration, The amount of non-overtime work assigned to the
        // resource.
        public string RegularWork { get; set; }

        // Duration, The amount of overtime work assigned to the
        // resource.
        public string OvertimeWork { get; set; }

        // The amount of actual work performed by the resource.
        public string ActualWork { get; set; }

        // The amount of remaining work required to complete all
        // assigned tasks.
        public string RemainingWork { get; set; }

        // The amount of actual overtime work performed by the
        // resource.
        public string ActualOvertimeWork { get; set; }

        // The amount of remaining overtime work required to complete
        // all tasks.
        public string RemainingOvertimeWork { get; set; }

        // The percentage of work completed across all tasks.
        public int PercentWorkComplete { get; set; }

        // The standard rate of the resource. This value is as of the
        // current date if a rate table exists for the resource.
        public decimal StandardRate { get; set; }

        // The units used by Microsoft Office Project to display the
        // standard rate.  1=m, 2=h, 3=d, 4=w, 5=mo, 7=y, 8=material
        // resource rate (or blank symbol specified).
        public int StandardRateFormat { get; set; }

        // The total project cost for the resource across all assigned
        // tasks.
        public decimal Cost { get; set; }

        // The overtime rate of the resource. This value is as of the
        // current date if a rate table exists for the resource.
        public decimal OvertimeRate { get; set; }

        // The units used by Microsoft Office Project to display the
        // overtime rate.  1=m, 2=h, 3=d, 4=w, 5=mo, 7=y.
        public int OvertimeRateFormat { get; set; }

        // The total overtime cost for the resource including actual
        // and remaining overtime costs.
        public decimal OvertimeCost { get; set; }

        // The cost per use of the resource. This value is as of the
        // current date if a rate table exists for the resource.
        public decimal CostPerUse { get; set; }

        // The actual cost incurred by the resource across all
        // assigned tasks.
        public decimal ActualCost { get; set; }

        // The actual overtime cost incurred by the resource across
        // all assigned tasks.
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

        // <xsd:element name="ExtendedAttribute" minOccurs="0" maxOccurs="unbounded">
        // <xsd:element name="Baseline" minOccurs="0" maxOccurs="unbounded">
        // <xsd:element name="OutlineCode" minOccurs="0" maxOccurs="unbounded">

        public bool IsCostResource { get; set; }

        public string AssnOwner { get; set; }

        public string AssnOwnerGuid { get; set; }

        public bool IsBudget { get; set; }

        // <xsd:element name="AvailabilityPeriods" minOccurs="0">
        // <xsd:element name="AvailabilityPeriod" minOccurs="0" maxOccurs="unbounded">

        // <xsd:element name="Rates" minOccurs="0">
        // <xsd:element name="Rate" minOccurs="0" maxOccurs="25">

        // <xsd:element name="TimephasedData" type="TimephasedDataType" minOccurs="0" maxOccurs="unbounded">
    }
}