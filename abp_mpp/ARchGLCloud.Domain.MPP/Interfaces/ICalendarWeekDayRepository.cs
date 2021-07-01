using System;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Domain.Core.Repositories;

namespace ARchGLCloud.Domain.MPP.Interfaces
{
    public interface ICalendarWeekDayRepository : IAddRepository<CalendarWeekDay, Guid>, IUpdateRepository<CalendarWeekDay, Guid>, IDeleteRepository<CalendarWeekDay, Guid>
    {
    }
}
