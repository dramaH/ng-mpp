using System;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Domain.Core.Repositories;

namespace ARchGLCloud.Domain.MPP.Interfaces
{
    public interface ITaskRepository : IAddRepository<Task, Guid>, IUpdateRepository<Task, Guid>, IDeleteRepository<Task, Guid>
    {
    }
}
