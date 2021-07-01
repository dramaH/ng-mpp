using ARchGLCloud.Domain.Core.Models;
using System;

namespace ARchGLCloud.Domain.Core.Repositories
{
    public interface ITenantRepository : IAddRepository<Tenant, Guid>, IUpdateRepository<Tenant, Guid>, IDeleteRepository<Tenant, Guid>
    {
        Tenant FindByTenantId(Guid tenantId);

        Tenant FindByHost(string name);
    }
}
