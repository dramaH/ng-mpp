using ARchGLCloud.Domain.Core.Validations;
using System;

namespace ARchGLCloud.Domain.Core.Commands
{
    public class AddTenantCommand : TenantCommand
    {
        public AddTenantCommand(Guid id, Guid tenantId, string eName, string name, string host, string connectionString) : base(id, tenantId,eName, name, host, connectionString)
        {
        }

        public override bool IsValid()
        {
            ValidationResult = new AddTenantCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
