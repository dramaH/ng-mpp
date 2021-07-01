using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.MPP.Models
{
    /// <summary>
    ///   The collection of calendars that is associated with the project.
    ///   Calendars are used to define standard working and
    ///   non-working times. Projects must have one base
    ///   calendar. Tasks and resources can have their own non-base
    ///   calendars that are based on a base calendar.
    /// </summary>
    [Table("Calendars", Schema = "mpp")]
    public class Calendar : MppAggregateRoot<Guid>
    {
        public Calendar() : base(Guid.NewGuid())
        {
            WeekDays = new List<CalendarWeekDay>();
            Exceptions = new List<CalendarException>();
        }

        public Calendar(Guid id) : base(id)
        {
            WeekDays = new List<CalendarWeekDay>();
            Exceptions = new List<CalendarException>();
        }

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
        [NotMapped]
        public List<CalendarWeekDay> WeekDays { get; set; }
        public string WeekDaysUUIDs { get; set; }
        [NotMapped]
        public List<CalendarException> Exceptions { get; set; }
        public string ExceptionsUUIDs { get; set; }

        // <!-- #New Project 2007 element definitions -->
        // FIXME

        //[ForeignKey("ParentId")]
        //public Project Project { get; set; }
    }
}
