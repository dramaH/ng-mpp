using System;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Domain.Core.Repositories;

namespace ARchGLCloud.Domain.MPP.Interfaces
{
    public interface IAssignmentRepository : IAddRepository<Assignment, Guid>, IUpdateRepository<Assignment, Guid>, IDeleteRepository<Assignment, Guid>
    {
    }
}
