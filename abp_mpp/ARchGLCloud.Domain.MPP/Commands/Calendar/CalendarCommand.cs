using ARchGLCloud.Domain.Core.Commands;
using ARchGLCloud.Domain.MPP.Models;
using System;
using System.Collections.Generic;

namespace ARchGLCloud.Domain.MPP.Commands
{
    public abstract class CalendarCommand : Command
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public int UID { get; set; }
        public string Name { get; set; }
        public bool IsBaseCalendar { get; set; }
        public int BaseCalendarUID { get; set; }
        public List<CalendarWeekDay> WeekDays { get; set; }
        public string WeekDaysUUIDs { get; set; }
        public List<CalendarException> Exceptions { get; set; }
        public string ExceptionsUUIDs { get; set; }
    }
}
