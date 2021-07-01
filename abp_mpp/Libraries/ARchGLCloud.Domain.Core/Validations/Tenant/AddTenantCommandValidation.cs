using ARchGLCloud.Domain.Core.Commands;

namespace ARchGLCloud.Domain.Core.Validations
{
    public class AddTenantCommandValidation : TenantValidation<AddTenantCommand>
    {
        public AddTenantCommandValidation()
        {
            this.ValidateTenantId();
            this.ValidateName();
            this.ValidateHost();
            this.ValidateConnectionString();
        }
    }
}
