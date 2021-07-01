using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.Core.Models
{
    [Table("AspNetUserClaims", Schema = "sso")]
    public class AspNetUserClaim : IdentityUserClaim<Guid>, IAggregateRoot<int>
    {
        public AspNetUserClaim()
        {
        }
    }
}
