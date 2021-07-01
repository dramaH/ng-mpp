using System;
using ARchGLCloud.Infra.Core.Repositories;
using ARchGLCloud.Domain.MPP.Interfaces;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Infra.MPP.Context;


namespace ARchGLCloud.Infra.MPP.Repositories
{
    public class TaskBaselineRepository : Repository<MPPContext, TaskBaseline, Guid>, ITaskBaselineRepository
    {
        public TaskBaselineRepository(MPPContext context) : base(context)
        {
        }
    }
}
