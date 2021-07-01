using System;
using ARchGLCloud.Infra.Core.Repositories;
using ARchGLCloud.Domain.MPP.Interfaces;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Infra.MPP.Context;

namespace ARchGLCloud.Infra.MPP.Repositories
{
    public class TaskPredecessorLinkRepository : Repository<MPPContext, TaskPredecessorLink, Guid>, ITaskPredecessorLinkRepository
    {
        public TaskPredecessorLinkRepository(MPPContext context)
            : base(context)
        {
        }
    }
}
