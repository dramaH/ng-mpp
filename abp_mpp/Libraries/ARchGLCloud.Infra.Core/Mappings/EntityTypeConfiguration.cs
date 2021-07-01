using ARchGLCloud.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ARchGLCloud.Infra.Core.Mappings
{
    public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IAggregateRoot
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property<bool>("SoftDeleted");

            builder.HasQueryFilter(entity => EF.Property<bool>(entity, "SoftDeleted") == false);
        }
    }
}
