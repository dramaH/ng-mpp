using System;
using ARchGLCloud.Infra.Core.Repositories;
using ARchGLCloud.Domain.MPP.Interfaces;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Infra.MPP.Context;


namespace ARchGLCloud.Infra.MPP.Repositories
{
    public class TaskExtendedAttributeRepository : Repository<MPPContext, TaskExtendedAttribute, Guid>, ITaskExtendedAttributeRepository
    {
        public TaskExtendedAttributeRepository(MPPContext context) : base(context)
        {
        }
    }
}
