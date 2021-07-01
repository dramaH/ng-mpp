using ARchGLCloud.Domain.Core.Events;
using ARchGLCloud.Domain.MPP.Models;
using System;
using System.Collections.Generic;

namespace ARchGLCloud.Domain.MPP.Events
{
    public class CalendarEvent : Event
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        // The unique identifier of the calendar.
        public int UID { get; set; }

        // The name of the calendar.
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
        public List<CalendarWeekDay> WeekDays { get; set; }
        public string WeekDaysUUIDs { get; set; }

        public List<CalendarException> Exceptions { get; set; }
        public string ExceptionsUUIDs { get; set; }
    }
}
