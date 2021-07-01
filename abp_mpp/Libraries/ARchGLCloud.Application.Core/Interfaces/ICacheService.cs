using ARchGLCloud.Application.Core.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ARchGLCloud.Application.Core.Interfaces
{
    public interface ICacheService<TKey, TQFilter, TResult> : IDisposable where TQFilter : QueryFilter
    {
        Task<TResult> GetAsync(string key);

        Task<List<TResult>> GetEntities(string key, int pageIndex, int pageSize);

        Task AddAsync(TResult entity);

        Task UpdateAsync(string key, TResult entity);

        Task RemoveAsync(string key);

        bool Exists(string key);
    }
}
