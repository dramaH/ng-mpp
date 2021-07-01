using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.MPP.Models
{
    /// <summary>
    ///   A weekday either defines regular days of the week or
    ///   exception days in the calendar.
    /// </summary>
    [Table("CalendarExceptions", Schema = "mpp")]
    public class CalendarExceptions : MppAggregateRoot<Guid>
    {
        public CalendarExceptions() : base(Guid.NewGuid()) { }
        public CalendarExceptions(Guid id) : base(id) { }

        // The type of day. Values are: 0=Exception, 1=Sunday,
        // 2=Monday, 3=Tuesday, 4=Wednesday, 5=Thursday, 6=Friday,
        // 7=Saturday.
        public int Type { get; set; }

        // Whether the specified date or day type is working.
        public bool DayWorking { get; set; }

        // 例外日期名称
        public string Name { get; set; }

        // 周期
        public int Period { get; set; }
        // The beginning of the exception time.
        public DateTime? FromDate { get; set; }
        // The end of the exception time.
        public DateTime? ToDate { get; set; }
        // }

    }
}