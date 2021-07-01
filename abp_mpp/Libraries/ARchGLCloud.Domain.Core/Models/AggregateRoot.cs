using System;
using System.ComponentModel.DataAnnotations;

namespace ARchGLCloud.Domain.Core.Models
{
    public abstract class AggregateRoot<TKey> : IAggregateRoot<TKey> where TKey : IEquatable<TKey>
    {
        [Key]
        public TKey Id { get; protected set; }

        protected AggregateRoot() : this(default(TKey))
        {
        }

        public AggregateRoot(TKey id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as AggregateRoot<TKey>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(AggregateRoot<TKey> a, AggregateRoot<TKey> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(AggregateRoot<TKey> a, AggregateRoot<TKey> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}