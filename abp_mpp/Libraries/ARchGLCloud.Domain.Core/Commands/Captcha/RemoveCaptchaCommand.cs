using ARchGLCloud.Domain.Core.Validations;
using System;

namespace ARchGLCloud.Domain.Core.Commands
{
    public class RemoveCaptchaCommand : CaptchaCommand
    {
        public RemoveCaptchaCommand()
        {
        }

        public RemoveCaptchaCommand(string key)
        {
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveCaptchaCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
