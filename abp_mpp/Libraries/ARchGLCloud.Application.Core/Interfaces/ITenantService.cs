using ARchGLCloud.Application.Core.Filters;
using ARchGLCloud.Application.Core.ViewModels;
using System;

namespace ARchGLCloud.Application.Core.Interfaces
{
    public interface ITenantService : IService<Guid, TenantQueryFilter, TenantViewModel>
    {
        TenantViewModel FindByTenantId(Guid tenantId);
        TenantViewModel FindByTenantHost(string host);
        string UpdateDatabase(string sql, string connectionString);
    }
}
