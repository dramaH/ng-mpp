using System;
using System.ComponentModel.DataAnnotations;

namespace ARchGLCloud.Application.Core.ViewModels
{
    public class TenantViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 租户ID
        /// </summary>
        [Required]
        public Guid TenantId { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 名称缩写
        /// </summary>
        [Required]
        [StringLength(10, ErrorMessage = "长度不能超过10")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "不能为中文")]
        public string EName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "长度不能超过100")]
        public string Name { get; set; }
        /// <summary>
        /// 宿主域名
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+$")]
        public string Host { get; set; }
        /// <summary>
        /// 数据库连接串
        /// </summary>
        [Required]
        public string ConnectionString { get; set; }
    }
}
