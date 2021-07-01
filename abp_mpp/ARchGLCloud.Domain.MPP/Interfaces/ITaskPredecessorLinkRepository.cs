using System;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Domain.Core.Repositories;

namespace ARchGLCloud.Domain.MPP.Interfaces
{
    public interface ITaskPredecessorLinkRepository: IAddRepository<TaskPredecessorLink, Guid>, IUpdateRepository<TaskPredecessorLink, Guid>, IDeleteRepository<TaskPredecessorLink, Guid>
    {
    }
}
