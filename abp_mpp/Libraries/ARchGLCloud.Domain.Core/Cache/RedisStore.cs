using ARchGLCloud.Domain.Core.Models;
using ARchGLCloud.Domain.Core.Repositories;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.Cache
{
    public abstract class RedisStore<TEntity> : IRedisStore<TEntity> where TEntity : RedisAggregateRoot
    {
        private readonly string _primary_key_prefix = "";
        private readonly ConnectionMultiplexer _redis;
        private readonly int _dbNumber = 1;

        protected readonly IConfiguration _configuration;
        protected readonly IDatabase _db;

        public RedisStore(IConfiguration configuration, int dbNumber, string prefix)
        {
            try
            {
                this._configuration = configuration;
                this._dbNumber = dbNumber;
                this._primary_key_prefix = prefix;

                StackExchangeRedisHelper.ConnctString = configuration.GetSection("IdentityOptions:Redis").Value;
                this._redis = StackExchangeRedisHelper.Instance;//ConnectionMultiplexer.Connect(configuration.GetSection("IdentityOptions:Redis").Value);
                this._db = _redis.GetDatabase(_dbNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected string GetPrimaryKeyName(string key)
        {
            return _primary_key_prefix + ":" + key;
        }

        public abstract Task AddAsync(TEntity entity);

        public bool Exists(string key)
        {
            return _db.KeyExists(GetPrimaryKeyName(key));
        }

        public virtual async Task UpdateAsync(string key, TEntity entity)
        {
            if (Exists(key))
            {
                var entries = HashEntries(entity);
                if (entries != null)
                {
                    await _db.HashSetAsync(GetPrimaryKeyName(key), entries);
                    await _db.KeyExpireAsync(GetPrimaryKeyName(key), TimeSpan.FromMinutes(entity.Expiration));
                }
            }
        }

        public virtual async Task<TEntity> GetAsync(string key)
        {
            if (Exists(key))
            {
                var items = await _db.HashGetAllAsync(GetPrimaryKeyName(key));
                return ToEntity(items);
            }
            else
            {
                return default;
            }
        }

        public virtual async Task<TEntity> GetRawAsync(string key)
        {
            var items = await _db.HashGetAllAsync(GetPrimaryKeyName(key));
            return ToEntity(items);
        }

        public async virtual Task RemoveAsync(string key)
        {
            if (Exists(key))
            {
                await _db.KeyDeleteAsync(GetPrimaryKeyName(key));
            }
        }

        protected abstract HashEntry[] HashEntries(TEntity entity);

        protected abstract TEntity ToEntity(HashEntry[] entries);

        public virtual void Dispose()
        {
            _redis.Dispose();
        }
    }
}
