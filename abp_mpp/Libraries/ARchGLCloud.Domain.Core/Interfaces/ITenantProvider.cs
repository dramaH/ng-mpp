using ARchGLCloud.Domain.Core.Models;

namespace ARchGLCloud.Domain.Core.Interfaces
{
    public interface ITenantProvider
    {
        Tenant Tenant { get; }
    }
}
