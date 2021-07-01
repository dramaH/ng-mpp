using System;

namespace ARchGLCloud.Domain.Core.Models
{
    public class RedisAggregateRoot : Entity
    {
        public RedisAggregateRoot()
        {
        }

        public RedisAggregateRoot(Guid id) : base(id)
        {
        }

        public int Expiration { get; set; }
    }
}
