using ARchGLCloud.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.Repositories
{
    public interface IUpdateRepository<TEntity, TKey> : IRepository<TEntity, TKey>
       where TEntity : IAggregateRoot<TKey>, IAggregateRoot where TKey : IEquatable<TKey>
    {
        void Update(TEntity entity);

        Task UpdateAsync(TEntity entity);

        void UpdateAll(IEnumerable<TEntity> entities);
    }
}