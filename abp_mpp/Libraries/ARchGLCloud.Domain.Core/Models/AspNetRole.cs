using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.Core.Models
{
    [Table("AspNetRoles", Schema = "sso")]
    public class AspNetRole : IdentityRole<Guid>, IAggregateRoot<Guid>
    {
        public AspNetRole() { }
        public AspNetRole(string roleName) : base(roleName) { }

        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
