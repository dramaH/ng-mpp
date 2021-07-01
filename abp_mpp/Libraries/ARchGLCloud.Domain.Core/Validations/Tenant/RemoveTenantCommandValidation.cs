using ARchGLCloud.Domain.Core.Commands;

namespace ARchGLCloud.Domain.Core.Validations
{
    public class RemoveTenantCommandValidation : TenantValidation<RemoveTenantCommand>
    {
        public RemoveTenantCommandValidation()
        {
            this.ValidateTenantId();
        }
    }
}
