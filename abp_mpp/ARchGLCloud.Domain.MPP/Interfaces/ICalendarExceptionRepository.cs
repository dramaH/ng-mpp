using System;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Domain.Core.Repositories;

namespace ARchGLCloud.Domain.MPP.Interfaces
{
    public interface ICalendarExceptionRepository : IAddRepository<CalendarException, Guid>, IUpdateRepository<CalendarException, Guid>, IDeleteRepository<CalendarException, Guid>
    {
    }
}
