using System;

namespace ARchGLCloud.Domain.Core.Models
{
    public interface IAggregateRoot<out TKey> : IAggregateRoot where TKey : IEquatable<TKey>
    {
        TKey Id { get; }
    }

    public interface IAggregateRoot
    {
    }
}