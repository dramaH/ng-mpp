using ARchGLCloud.Domain.Core.Commands;
using FluentValidation;
using System;

namespace ARchGLCloud.Domain.Core.Validations
{
    public class Validation<T> : AbstractValidator<T> where T : Command
    {
        protected void ValidateMessage(string msgType)
        {
            RuleFor(c => c.MessageType).NotNull().NotEmpty().WithMessage("消息类型必须存在");
        }
    }
}
