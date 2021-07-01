using ARchGLCloud.Domain.Core.Validations;
using System;

namespace ARchGLCloud.Domain.Core.Commands
{
    public class UpdateTenantCommand : TenantCommand
    {
        public UpdateTenantCommand(Guid id, Guid tenantId, string eName, string name, string host, string connectionString) : base(id, tenantId, eName, name,host, connectionString)
        {
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateTenantCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
