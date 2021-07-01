using ARchGLCloud.Domain.Core.Validations;
using System;

namespace ARchGLCloud.Domain.Core.Commands
{
    public class RemoveTenantCommand : TenantCommand
    {
        public RemoveTenantCommand(Guid id) : base(id, Guid.Empty, null, null, null, null)
        {
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveTenantCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
