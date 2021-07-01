using ARchGLCloud.Domain.Core.Models;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.Repositories
{
    public interface IRedisStore<TEntity> where TEntity : RedisAggregateRoot
    {
        Task<TEntity> GetAsync(string key);

        Task AddAsync(TEntity entity);

        Task UpdateAsync(string key, TEntity entity);

        Task RemoveAsync(string key);

        void Dispose();

        bool Exists(string key);
    }
}
