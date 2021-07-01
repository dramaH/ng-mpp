using ARchGLCloud.Domain.Core.Interfaces;
using ARchGLCloud.Domain.Core.Models;
using ARchGLCloud.Domain.Core.Repositories;
using Microsoft.AspNetCore.Http;
using System;

namespace ARchGLCloud.Infra.Core.Provider
{
    public class TenantProvider : ITenantProvider
    {
        public TenantProvider(IHttpContextAccessor accessor, ITenantRepository repository)
        {
            Guid.TryParse(accessor.HttpContext.Request.Query["tenantid"], out Guid tenantId);
            if (tenantId.Equals(Guid.Empty))
            {
                Guid.TryParse(accessor.HttpContext.Request.Headers["TenantId"], out tenantId);

                if (tenantId.Equals(Guid.Empty))
                {
                    throw new ArgumentNullException();
                }
            }

            Tenant = repository.FindByTenantId(tenantId);
        }

        public Tenant Tenant { get; private set;}
    }
}
