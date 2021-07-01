using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.MPP.Models
{
    /// <summary>
    ///   A weekday either defines regular days of the week or
    ///   exception days in the calendar.
    /// </summary>
    [Table("CalendarWeekDays", Schema = "mpp")]
    public class CalendarWeekDay : MppAggregateRoot<Guid>
    {
        public CalendarWeekDay() : base(Guid.NewGuid()) { }
        public CalendarWeekDay(Guid id) : base(id) { }

        // The type of day. Values are: 0=Exception, 1=Sunday,
        // 2=Monday, 3=Tuesday, 4=Wednesday, 5=Thursday, 6=Friday,
        // 7=Saturday.
        public int DayType { get; set; }

        // Whether the specified date or day type is working.
        public bool DayWorking { get; set; }

        // Defines a contiguous set of exception days.
        // <xsd:element name="TimePeriod" minOccurs="0">

        // public struct TimePeriod
        // {
        // The beginning of the exception time.
        public DateTime? FromDate { get; set; }
        // The end of the exception time.
        public DateTime? ToDate { get; set; }
        // }

        // <xsd:element name="WorkingTimes" minOccurs="0">
        // The collection of working times that define the time worked
        // on the weekday. One of these must be present, and there can
        // be no more than five.
        // <xsd:element name="WorkingTime" minOccurs="0" maxOccurs="5">

        // public struct WorkingTimes
        // {
        //     public struct WorkingTime_1
        //     {
        // The beginning of the working time. Time only
        public DateTime? FromTime_0 { get; set; }

        // The end of the working time. Time only
        public DateTime? ToTime_0 { get; set; }
        // }

        // public struct WorkingTime_2
        // {
        // The beginning of the working time. Time only
        public DateTime? FromTime_1 { get; set; }

        // The end of the working time. Time only
        public DateTime? ToTime_1 { get; set; }
        // }

        // public struct WorkingTime_3
        // {
        // The beginning of the working time. Time only
        public DateTime? FromTime_2 { get; set; }

        // The end of the working time. Time only
        public DateTime? ToTime_2 { get; set; }
        // }

        // public struct WorkingTime_4
        // {
        // The beginning of the working time. Time only
        public DateTime? FromTime_3 { get; set; }

        // The end of the working time. Time only
        public DateTime? ToTime_3 { get; set; }
        // }

        // public struct WorkingTime_5
        // {
        // The beginning of the working time. Time only
        public DateTime? FromTime_4 { get; set; }

        // The end of the working time. Time only
        public DateTime? ToTime_4 { get; set; }
        //     }
        // }
    }
}