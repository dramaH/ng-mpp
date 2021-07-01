using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.MPP.Models
{
    /// <summary>
    ///   The collection of exceptions that is associated with the calendar.
    ///   Exceptions are used to define an exception sub-calendar.
    /// </summary>
    [Table("CalendarExceptions", Schema = "mpp")]
    public class CalendarException: MppAggregateRoot<Guid>
    {
        public CalendarException() : base(Guid.NewGuid()) { }
        public CalendarException(Guid id) : base(id) { }

        // Whether the range of recurrence is defined by entering a
        // number of occurrences. False specifies that the range of
        // recurrence is defined by entering a finish date.
        public bool EnteredByOccurrences { get; set; }

        // Defines a contiguous set of exception days.
        // <xsd:element name="TimePeriod" minOccurs="0">

        // public struct TimePeriod
        // {
        // The beginning of the exception time.
        public DateTime? FromDate { get; set; }
        // The end of the exception time.
        public DateTime? ToDate { get; set; }
        // }

        // The number of occurrences for which the calendar exception
        // is valid.
        public int Occurrences { get; set; }

        // The name of the exception.
        [MaxLength(512)]
        public string Name { get; set; }

        // The exception type. Values are: 1=Daily, 2=Yearly by day of
        // the month, 3=Yearly by position, 4=Monthly by day of the
        // month, 5=Monthly by position, 6=Weekly, 7=By day count,
        // 8=By weekday count, 9=No exception type.
        public int Type { get; set; }

        // The period of recurrence for the exception.
        public int Period { get; set; }

        // The days of the week on which the exception is
        // valid. Values are: 1=Sunday, 2=Monday, 4=Tuesday,
        // 8=Wednesday, 16=Thursday, 32=Friday, 64=Saturday.
        public int DasyOfWeek { get; set; }

        // The month item for which an exception recurrence is
        // scheduled. Values are: 0=Day, 1=Weekday, 2=WeekendDay,
        // 3=Sunday, 4=Monday, 5=Tuesday, 6=Wednesday, 7=Thursday,
        // 8=Friday, 9=Saturday.
        public int MonthIten { get; set; }

        // The position of a month item within a month. Values are:
        // 0=First position, 1=Second position, 2=Third position,
        // 3=Fourth position, 4=Last position.
        public int MonthPosition { get; set; }

        // The month for which an exception recurrence is
        // scheduled. Values are: 0=January, 1=February, 2=March,
        // 3=April, 4=May, 5=June, 6=July, 7=August, 8=September,
        // 9=October, 10=November, 11=December.
        public int Month { get; set; }

        // The day of the month on which an exception recurrence is
        // scheduled.
        public int MonthDay { get; set; }

        // Whether the specified date or day type is working.
        public bool DayWorking { get; set; }

        // <xsd:element name="WorkingTimes" minOccurs="0">
        //  <xsd:element name="WorkingTime" minOccurs="0" maxOccurs="5">
        public DateTime? FromTime_0 { get; set; }
        public DateTime? ToTime_0 { get; set; }

        public DateTime? FromTime_1 { get; set; }
        public DateTime? ToTime_1 { get; set; }

        public DateTime? FromTime_2 { get; set; }
        public DateTime? ToTime_2 { get; set; }

        public DateTime? FromTime_3 { get; set; }
        public DateTime? ToTime_3 { get; set; }

        public DateTime? FromTime_4 { get; set; }
        public DateTime? ToTime_4 { get; set; }
    }
}