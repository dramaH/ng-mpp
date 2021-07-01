using ARchGLCloud.Domain.Core.Validations;

namespace ARchGLCloud.Domain.Core.Commands
{
    public class AddCaptchaCommand : CaptchaCommand
    {
        public AddCaptchaCommand()
        {
        }

        public override bool IsValid()
        {
            ValidationResult = new AddCaptchaCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
