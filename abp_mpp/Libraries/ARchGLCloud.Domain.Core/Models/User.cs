using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ARchGLCloud.Domain.Core.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.Http;

namespace ARchGLCloud.Domain.Core.Models
{
    public class User : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public User(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsInRole(string name)
        {
            return _accessor.HttpContext.User.IsInRole(name);
        }

        public IEnumerable<Claim> IdentityClaims
        {
            get
            {
                return _accessor.HttpContext.User.Claims;
            }
        }
        public string Authorization
        {
            get
            {
                return _accessor.HttpContext.Request.Headers["Authorization"];
            }
        }
        public string Id => IdentityClaims?.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject)?.Value;
    }
}
