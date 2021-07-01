using ARchGLCloud.Domain.Core.Commands;

namespace ARchGLCloud.Domain.Core.Validations
{
    public class RemoveCaptchaCommandValidation : CaptchaValidation<RemoveCaptchaCommand>
    {
        public RemoveCaptchaCommandValidation()
        {
            this.ValidatePhone();
        }
    }
}
