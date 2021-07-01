using System;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Domain.Core.Repositories;

namespace ARchGLCloud.Domain.MPP.Interfaces
{
    public interface IProjectRepository : IAddRepository<Project, Guid>, IUpdateRepository<Project, Guid>, IDeleteRepository<Project, Guid>
    {
    }
}
