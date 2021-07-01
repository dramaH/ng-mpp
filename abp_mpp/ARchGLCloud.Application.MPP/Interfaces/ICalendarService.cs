using ARchGLCloud.Application.Core.Interfaces;
using ARchGLCloud.Application.MPP.Dtos;
using ARchGLCloud.Domain.MPP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Application.MPP.Interfaces
{
    public interface ICalendarService 
    { 
        List<CalendarExceptionDto> GetCalendarExceptions(Guid calendarId);

        List<CalendarWeekDayDto> GetCalendarWeekDays(Guid calendarId);

        IEnumerable<Calendar> GetCalendars(Guid projectId);

        void UpdateException(CalendarExceptionDto input);

        void AddException(CalendarExceptionDto input);

        void UpdateWeekDays(Guid parentId, List<CalendarWeekDayDto> input);

        void DeleteException(Guid id);

    }
}
