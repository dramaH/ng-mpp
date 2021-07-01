using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Domain.Core.Interfaces
{
    public interface ISoftDelete
    {
        void SoftDelete(EntityEntry entry);
    }
}
