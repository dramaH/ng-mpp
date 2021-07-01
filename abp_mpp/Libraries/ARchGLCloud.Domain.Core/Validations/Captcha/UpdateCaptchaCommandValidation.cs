using ARchGLCloud.Domain.Core.Commands;

namespace ARchGLCloud.Domain.Core.Validations
{
    public class UpdateCaptchaCommandValidation : CaptchaValidation<UpdateCaptchaCommand>
    {
        public UpdateCaptchaCommandValidation()
        {
            this.ValidateId();
            this.ValidatePhone();
            this.ValidateCode();
            this.ValidateExpiration();
        }
    }
}
