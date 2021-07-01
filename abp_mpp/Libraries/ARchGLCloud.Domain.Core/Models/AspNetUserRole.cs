using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.Core.Models
{
    [Table("AspNetUserRoles", Schema = "sso")]
    public class AspNetUserRole : IdentityUserRole<Guid>
    {
        public AspNetUserRole()
        {
        }

        public AspNetUserRole(Guid userId, Guid roleId)
        {
            this.UserId = userId;
            this.RoleId = roleId;
        }
    }
}
