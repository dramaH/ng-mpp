using System;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Domain.Core.Repositories;

namespace ARchGLCloud.Domain.MPP.Interfaces
{
    public interface ITaskExtendedAttributeRepository: IAddRepository<TaskExtendedAttribute, Guid>, IUpdateRepository<TaskExtendedAttribute, Guid>, IDeleteRepository<TaskExtendedAttribute, Guid>
    {
    }
}
