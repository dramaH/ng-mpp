using Microsoft.AspNetCore.Identity;
using System;

namespace ARchGLCloud.Domain.Core.Models
{
    public class AspNetRoleClaim : IdentityRoleClaim<Guid>, IAggregateRoot<int>
    {
    }
}
