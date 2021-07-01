using ARchGLCloud.Application.Core.Filters;
using System;
using System.Collections.Generic;

namespace ARchGLCloud.Application.Core.Interfaces
{
    public interface IService<TKey, TQFilter, TResult> : IDisposable where TQFilter : QueryFilter
    {
        void Add(TResult viewModel);
        void Update(TResult viewModel);
        void Remove(TKey id);

        TResult Find(TKey id);
        IEnumerable<TResult> FindAll();
        IEnumerable<TResult> Pager(TQFilter filter,out int count);
    }
}
