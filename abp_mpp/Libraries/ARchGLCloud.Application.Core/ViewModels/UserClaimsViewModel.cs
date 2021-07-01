using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Application.Core
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Role = new List<string>();
        }
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 公司名字
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// role
        /// </summary>
        public List<string> Role { get; set; }

    }
}
