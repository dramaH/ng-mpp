using ARchGLCloud.Domain.Core.Events;
using System;

namespace ARchGLCloud.Domain.MPP.Events
{
    public class CalendarExceptionEvent : Event
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public bool EnteredByOccurrences { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int Occurrences { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public int Period { get; set; }

        public int DasyOfWeek { get; set; }

        public int MonthIten { get; set; }

        public int MonthPosition { get; set; }

        public int Month { get; set; }

        public int MonthDay { get; set; }

        public bool DayWorking { get; set; }

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
