using ARchGLCloud.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.Repositories
{
    public interface IAddRepository<TEntity, TKey> : IRepository<TEntity, TKey>
       where TEntity : IAggregateRoot<TKey>, IAggregateRoot where TKey : IEquatable<TKey>
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
    }
}