using System.Linq;
using ARchGLCloud.Domain.Core.Repositories;
using ARchGLCloud.Domain.Core.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace ARchGLCloud.Infra.Core.Repositories
{
    public class TenantRepository<T> : Repository<T, Tenant, Guid>, ITenantRepository where T : DbContext
    {
        public TenantRepository(T context) : base(context)
        {
        }

        public Tenant FindByHost(string name)
        {
            var tenant = dbSet.FirstOrDefault(t => t.Host == name);
            return tenant;
        }

        public Tenant FindByTenantId(Guid tenantId)
        {
            var tenant = dbSet.FirstOrDefault(t => t.TenantId == tenantId);
            return tenant;
        }
    }
}
