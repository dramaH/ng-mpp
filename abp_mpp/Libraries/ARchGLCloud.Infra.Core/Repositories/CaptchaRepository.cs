using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ARchGLCloud.Domain.Core.Cache;
using ARchGLCloud.Domain.Core.Models;
using ARchGLCloud.Domain.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ARchGLCloud.Infra.Core.Repositories
{
    public class CaptchaRepository : RedisStore<Captcha>, ICaptchaRepository
    {
        public CaptchaRepository(IConfiguration configuration) : base(configuration, 9, "spiderbim")
        {
        }

        public override async Task AddAsync(Captcha entity)
        {
            await _db.HashSetAsync(GetPrimaryKeyName(entity.Phone), HashEntries(entity));
            await _db.KeyExpireAsync(GetPrimaryKeyName(entity.Phone), TimeSpan.FromMinutes(entity.Expiration));
            await _db.ListRightPushAsync(GetPrimaryKeyName(entity.IP), JsonConvert.SerializeObject(entity));
            await _db.KeyPersistAsync(GetPrimaryKeyName(entity.IP));
        }

        public async Task<List<string>> GetEntities(string key, int start, int stop)
        {
            var keys = await _db.ListRangeAsync(GetPrimaryKeyName(key), start, stop);

            var captchas = new List<string>();
            foreach (string k in keys)
            {
                captchas.Add(k);
            }

            return captchas;
        }

        protected override HashEntry[] HashEntries(Captcha entity)
        {
            return new HashEntry[]
            {
                new HashEntry("id", entity.Id.ToString()),
                new HashEntry("phone", entity.Phone),
                new HashEntry("ip", entity.IP),
                new HashEntry("subject", entity.Subject),
                new HashEntry("code", entity.Code),
                new HashEntry("createTime", entity.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")),
                new HashEntry("expiration", entity.Expiration)
            };
        }

        protected override Captcha ToEntity(HashEntry[] entries)
        {
            if (entries.Length == 0)
            {
                return null;
            }

            var id = entries.FirstOrDefault(i => i.Name == "id");
            var entity = new Captcha(Guid.Parse(id.Value));

            foreach (var item in entries)
            {
                if(item.Name == "phone")
                {
                    entity.Phone = item.Value;
                }
                if (item.Name == "ip")
                {
                    entity.IP = item.Value;
                }
                if (item.Name == "subject")
                {
                    entity.Subject = item.Value;
                }
                if(item.Name == "code")
                {
                    entity.Code = item.Value;
                }
                if (item.Name == "createTime")
                {
                    entity.CreateTime = DateTime.Parse(item.Value);
                }
                if (item.Name == "expiration")
                {
                    entity.Expiration = int.Parse(item.Value);
                }
            }

            return entity;
        }
    }
}
