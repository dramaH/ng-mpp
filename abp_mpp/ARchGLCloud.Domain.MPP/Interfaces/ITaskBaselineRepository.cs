using System;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Domain.Core.Repositories;

namespace ARchGLCloud.Domain.MPP.Interfaces
{
    public interface ITaskBaselineRepository: IAddRepository<TaskBaseline, Guid>, IUpdateRepository<TaskBaseline, Guid>, IDeleteRepository<TaskBaseline, Guid>
    {
    }
}
