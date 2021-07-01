using System.ComponentModel.DataAnnotations;

namespace ARchGLCloud.Application.Core.ViewModels
{
    public class CheckTelephoneViewModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required,RegularExpression(@"^1[3456789]\d{9}$", ErrorMessage = "请输入正确的手机号码")]
        public string Phone { get; set; }
    }
}
