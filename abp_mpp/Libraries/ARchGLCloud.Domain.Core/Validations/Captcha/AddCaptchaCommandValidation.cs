using ARchGLCloud.Domain.Core.Commands;

namespace ARchGLCloud.Domain.Core.Validations
{
    public class AddCaptchaCommandValidation : CaptchaValidation<AddCaptchaCommand>
    {
        public AddCaptchaCommandValidation()
        {
            this.ValidatePhone();
            this.ValidateCode();
            this.ValidateExpiration();
        }
    }
}
