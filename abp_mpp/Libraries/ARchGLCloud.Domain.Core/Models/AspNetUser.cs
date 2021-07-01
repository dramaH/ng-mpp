using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.Core.Models
{
    [Table("AspNetUsers", Schema = "sso")]
    public class AspNetUser : IdentityUser<Guid>, IAggregateRoot<Guid>
    {
        public AspNetUser() { }
        public AspNetUser(string userName) : base(userName) { }

        public string RealName { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
