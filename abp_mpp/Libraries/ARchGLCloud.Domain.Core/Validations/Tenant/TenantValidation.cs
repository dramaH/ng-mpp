using ARchGLCloud.Domain.Core.Commands;
using FluentValidation;
using System;

namespace ARchGLCloud.Domain.Core.Validations
{
    public class TenantValidation<T> : AbstractValidator<T> where T : TenantCommand
    {
        protected void ValidateId()
        {
            RuleFor(c => c.Id).NotEqual(Guid.Empty).WithMessage("ID必须存在");
        }

        protected void ValidateTenantId()
        {
            RuleFor(c => c.TenantId).NotEqual(Guid.Empty).WithMessage("租户ID不能为空");
        }

        protected void ValidateName()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty().WithMessage("名字不能为空")
                .Length(1, 100).WithMessage("名字长度必须在1~1000之间");
        }

        protected void ValidateHost()
        {
            RuleFor(c => c.Host).NotNull().NotEmpty().WithMessage("域名必须存在");
        }

        protected void ValidateConnectionString()
        {
            RuleFor(c => c.ConnectionString).NotNull().NotEmpty().WithMessage("模型路径长度必须小于255");
        }
    }
}
