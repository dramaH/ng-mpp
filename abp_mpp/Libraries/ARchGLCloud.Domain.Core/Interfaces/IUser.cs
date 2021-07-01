using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ARchGLCloud.Domain.Core.Interfaces
{
    public interface IUser
    {
        string Id { get; }
        string Name { get; }
        string Authorization { get; }
        bool IsAuthenticated();
        bool IsInRole(string name);
        IEnumerable<Claim> IdentityClaims { get; }
    }
}
