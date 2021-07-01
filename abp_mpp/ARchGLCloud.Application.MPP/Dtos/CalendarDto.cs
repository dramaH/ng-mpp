using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ARchGLCloud.Application.MPP.Dtos
{
    public class CalendarDto
    {
        public Guid Id { get; set; }
        // The unique identifier of the calendar.
        public int UID { get; set; }

        // The name of the calendar.
        [MaxLength(512)]
        public string Name { get; set; }

        // Whether the calendar is a base calendar.
        public bool IsBaseCalendar { get; set; }

        // The unique identifier of the base calendar on which this
        // calendar depends. Only applicable if the calendar is not a
        // base calendar.
        public int BaseCalendarUID { get; set; }

        // <xsd:element name="WeekDays" minOccurs="0">
        // The collection of weekdays that defines this calendar.
        // <xsd:element name="WeekDay" minOccurs="0" maxOccurs="unbounded">

        public List<CalendarWeekDayDto> WeekDays { get; set; }
        public string WeekDaysUUIDs { get; set; }

        public List<CalendarExceptionDto> Exceptions { get; set; }
        public string ExceptionsUUIDs { get; set; }
    }
}
