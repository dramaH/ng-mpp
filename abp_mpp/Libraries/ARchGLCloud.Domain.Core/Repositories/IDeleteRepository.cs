using ARchGLCloud.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.Repositories
{
    public interface IDeleteRepository<TEntity, TKey> : IRepository<TEntity, TKey>
       where TEntity : IAggregateRoot<TKey>, IAggregateRoot where TKey : IEquatable<TKey>
    {
        void Delete(TKey id);
        Task DeleteAsync(TKey id);

        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);

        void SoftDelete(TEntity entity);
        Task SoftDeleteAsync(TEntity entity);

        void DeleteAll(IEnumerable<TEntity> entities);
    }
}