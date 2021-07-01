using ARchGLCloud.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Domain.Core.Validations
{
    public class UpdateTenantCommandValidation : TenantValidation<UpdateTenantCommand>
    {
        public UpdateTenantCommandValidation()
        {
            this.ValidateId();
            this.ValidateTenantId();
            this.ValidateName();
            this.ValidateHost();
            this.ValidateConnectionString();
        }
    }
}
