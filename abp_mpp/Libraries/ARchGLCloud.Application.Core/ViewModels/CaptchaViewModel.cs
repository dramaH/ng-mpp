using System;
using System.ComponentModel.DataAnnotations;

namespace ARchGLCloud.Application.Core.ViewModels
{
    public class CaptchaViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [RegularExpression(@"^1[3456789]\d{9}$", ErrorMessage = "请输入正确的手机号码")]
        [Display(Name = "手机号码")]
        public string Phone { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        [StringLength(6)]
        public string Code { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [StringLength(10)]
        public string SignName { get; set; }
        /// <summary>
        /// 模板编码
        /// </summary>
        [StringLength(20)]
        public string TemplateCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; } = DateTimeOffset.Now;
        /// <summary>
        /// 有效期（分钟）
        /// </summary>
        public int Expiration { get; set; }

        /// <summary>
        /// 是否对手机号进行重复性检查
        /// </summary>
        public bool IsCheckPhone { get; set; }
    }
}
