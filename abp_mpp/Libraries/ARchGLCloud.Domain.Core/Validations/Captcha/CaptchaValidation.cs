using ARchGLCloud.Domain.Core.Commands;
using FluentValidation;
using System;

namespace ARchGLCloud.Domain.Core.Validations
{
    public class CaptchaValidation<T> : AbstractValidator<T> where T : CaptchaCommand
    {
        protected void ValidateId()
        {
            RuleFor(c => c.Id).NotEqual(Guid.Empty).WithMessage("ID必须存在");
        }

        protected void ValidatePhone()
        {
            RuleFor(c => c.Phone).NotNull().NotEmpty().Length(10, 20).WithMessage("手机号码不能为空");
        }

        protected void ValidateIP()
        {
            RuleFor(c => c.IP).NotNull().NotEmpty().Length(5, 20).WithMessage("IP地址不能为空");
        }

        protected void ValidateCode()
        {
            RuleFor(c => c.Code).NotNull().NotEmpty().WithMessage("验证码不能为空").Length(6).WithMessage("名字长度必须在1~6之间");
        }

        protected void ValidateExpiration()
        {
            RuleFor(c => c.Expiration).GreaterThanOrEqualTo(1).WithMessage("过期时间不能为空");
        }
    }
}
