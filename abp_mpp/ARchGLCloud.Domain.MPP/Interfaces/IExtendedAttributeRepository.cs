using System;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Domain.Core.Repositories;

namespace ARchGLCloud.Domain.MPP.Interfaces
{
    public interface IExtendedAttributeRepository : IAddRepository<ExtendedAttribute, Guid>, IUpdateRepository<ExtendedAttribute, Guid>, IDeleteRepository<ExtendedAttribute, Guid>
    {
    }
}
