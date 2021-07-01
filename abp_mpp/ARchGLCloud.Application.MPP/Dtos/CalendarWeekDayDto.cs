using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Application.MPP.Dtos
{
    public class CalendarWeekDayDto
    {
        public Guid Id { get; set; }

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

    }
}
