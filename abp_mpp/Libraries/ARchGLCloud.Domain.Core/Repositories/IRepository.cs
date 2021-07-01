using ARchGLCloud.Core;
using ARchGLCloud.Domain.Core.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.Repositories
{
    public interface IRepository<TEntity, TKey> : IDisposable where TEntity : IAggregateRoot<TKey>, IAggregateRoot where TKey : IEquatable<TKey>
    {
        TEntity Find(TKey id);
        Task<TEntity> FindAsync(TKey id);

        int Count();

        IQueryable<TEntity> FindAll();
        Task<IQueryable<TEntity>> FindAllAsync();

        IQueryable<TEntity> Pager(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize,out int count);
        Task<IQueryable<TEntity>> PagerAsync(Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, out int count);

        IQueryable<TEntity> Pager(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize,OrderParam[] orderParams, out int count);
        Task<IQueryable<TEntity>> PagerAsync(Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, OrderParam[] orderParams, out int count);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        int ExecuteSql(string sql, params object[] parameters);

        void ExecuteTransaction(Action transactionBegin, Action transactionEnd);

        int SaveChanges();
        Task<int> SaveChangesAsync();

    }
}