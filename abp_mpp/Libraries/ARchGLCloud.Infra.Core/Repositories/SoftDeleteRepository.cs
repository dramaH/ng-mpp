using ARchGLCloud.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Infra.Core.Repositories
{
    public class SoftDeleteRepository : ISoftDelete
    {
        public void SoftDelete(EntityEntry entry)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.CurrentValues["SoftDeleted"] = false;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["SoftDeleted"] = true;
                    break;
            }
        }
    }
}
