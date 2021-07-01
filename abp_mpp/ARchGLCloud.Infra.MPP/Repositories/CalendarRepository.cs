using System;
using ARchGLCloud.Infra.Core.Repositories;
using ARchGLCloud.Domain.MPP.Interfaces;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Infra.MPP.Context;

namespace ARchGLCloud.Infra.MPP.Repositories
{
    public class CalendarRepository : Repository<MPPContext, Calendar, Guid> , ICalendarRepository
    {
        public CalendarRepository(MPPContext context) : base(context)
        {
        }
    }
}
