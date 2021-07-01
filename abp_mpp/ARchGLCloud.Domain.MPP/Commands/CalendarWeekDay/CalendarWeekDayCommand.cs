using ARchGLCloud.Domain.Core.Commands;
using System;

namespace ARchGLCloud.Domain.MPP.Commands
{
    public abstract class CalendarWeekDayCommand : Command
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int DayType { get; set; }

        public bool DayWorking { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

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
