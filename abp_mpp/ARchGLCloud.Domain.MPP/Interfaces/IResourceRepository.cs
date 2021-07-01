using System;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Domain.Core.Repositories;

namespace ARchGLCloud.Domain.MPP.Interfaces
{
    public interface IResourceRepository : IAddRepository<Resource, Guid>, IUpdateRepository<Resource, Guid>, IDeleteRepository<Resource, Guid>
    {
    }
}
