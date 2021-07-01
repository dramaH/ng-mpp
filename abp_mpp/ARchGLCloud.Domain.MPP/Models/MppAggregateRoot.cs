using ARchGLCloud.Domain.Core.Models;
using System;

namespace ARchGLCloud.Domain.MPP.Models
{
    public class MppAggregateRoot<TKey> : AggregateRoot<TKey> where TKey : IEquatable<TKey>
    {
        public MppAggregateRoot()
        {
        }

        public MppAggregateRoot(TKey id) : base(id)
        {
        }

        public bool Enabled { get; set; } = true;
 
        public TKey ParentId { get; set; }
    }
}
