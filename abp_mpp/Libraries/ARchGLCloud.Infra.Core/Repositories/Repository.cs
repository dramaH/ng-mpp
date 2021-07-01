using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ARchGLCloud.Core;
using ARchGLCloud.Domain.Core.Models;
using ARchGLCloud.Domain.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ARchGLCloud.Infra.Core.Repositories
{
    public class Repository<TDbContext, TEntity, TKey> : IAddRepository<TEntity, TKey>, IUpdateRepository<TEntity, TKey>, IDeleteRepository<TEntity, TKey> where TDbContext : DbContext where TEntity : class, IAggregateRoot<TKey> where TKey : IEquatable<TKey>
    {
        protected readonly TDbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public Repository(TDbContext context)
        {
            this.context = context;
            dbSet = this.context.Set<TEntity>();
        }

        public int Count()
        {
            return dbSet.Count();
        }

        public virtual TEntity Find(TKey id)
        {
            return dbSet.Find(id);
        }

        public virtual Task<TEntity> FindAsync(TKey id)
        {
            return dbSet.FindAsync(id);
        }

        public virtual IQueryable<TEntity> FindAll()
        {
            return dbSet.AsNoTracking();
        }

        public virtual Task<IQueryable<TEntity>> FindAllAsync()
        {
            return Task.FromResult(dbSet.AsNoTracking());
        }

        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return dbSet.AsNoTracking().Where(expression);
        }

        public IQueryable<TEntity> Pager(Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, out int count)
        {
            //返回具体行数
            count = dbSet.Where(expression).Count();
            return dbSet.Where(expression).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<TEntity> Pager(Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, OrderParam[] orderParams, out int count)
        {
            var queryable = dbSet.Where(expression);
            count = queryable.Count();
            var _orderParames = Expression.Parameter(typeof(TEntity), "o");
            if (orderParams != null && orderParams.Length > 0)
            {
                for (int i = 0; i < orderParams.Length; i++)
                {
                    //根据属性名获取属性
                    var _property = typeof(TEntity).GetProperty(orderParams[i].PropertyName);
                    //创建一个访问属性的表达式
                    var _propertyAccess = Expression.MakeMemberAccess(_orderParames, _property);
                    var _orderByExp = Expression.Lambda(_propertyAccess, _orderParames);
                    string _orderName = orderParams[i].Method == OrderMethod.ASC ? "OrderBy" : "OrderByDescending";
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), _orderName, new Type[] { typeof(TEntity), _property.PropertyType }, queryable.Expression, Expression.Quote(_orderByExp));
                    queryable = queryable.Provider.CreateQuery<TEntity>(resultExp);
                }
            }

            return queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public Task<IQueryable<TEntity>> PagerAsync(Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, out int count)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> PagerAsync(Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, OrderParam[] orderParams, out int count)
        {
            throw new NotImplementedException();
        }

        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual Task AddAsync(TEntity entity)
        {
            return dbSet.AddAsync(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            return dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateAll(IEnumerable<TEntity> entities)
        {
            dbSet.UpdateRange(entities);
        }

        public virtual void Delete(TKey id)
        {
            dbSet.Remove(dbSet.Find(id));
        }

        public virtual Task DeleteAsync(TKey id)
        {
            return Task.Factory.StartNew(() => dbSet.Remove(dbSet.Find(id)));
        }

        public virtual void SoftDelete(TEntity entity)
        {
            var entry = context.Entry(entity);
            entry.CurrentValues["SoftDeleted"] = true;
        }

        public virtual Task SoftDeleteAsync(TEntity entity)
        {
            var entry = context.Entry(entity);
            entry.CurrentValues["SoftDeleted"] = true;
            return Task.CompletedTask;
        }

        public virtual void DeleteAll(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public virtual int SaveChanges()
        {
            return context.SaveChanges();
        }

        public virtual Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }

        public int ExecuteSql(string sql, params object[] parameters)
        {
            return context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public void Delete(TEntity entity)
        {
            context.Remove(entity);
        }

        public Task DeleteAsync(TEntity entity)
        {
            return Task.FromResult(context.Remove(entity));
        }

        public void ExecuteTransaction(Action transactionBegin, Action transactionEnd)
        {
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    transactionBegin?.Invoke();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    transactionEnd?.Invoke();
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
