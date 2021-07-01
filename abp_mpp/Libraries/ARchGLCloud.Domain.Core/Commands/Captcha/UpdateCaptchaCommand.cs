using ARchGLCloud.Domain.Core.Validations;
using System;

namespace ARchGLCloud.Domain.Core.Commands
{
    public class UpdateCaptchaCommand : CaptchaCommand
    {
        public UpdateCaptchaCommand()
        {
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateCaptchaCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
