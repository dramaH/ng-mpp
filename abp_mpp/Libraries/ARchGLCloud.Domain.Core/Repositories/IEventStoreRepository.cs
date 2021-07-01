using ARchGLCloud.Domain.Core.Events;
using System;
using System.Collections.Generic;

namespace ARchGLCloud.Domain.Core.Repositories
{
    public interface IEventStoreRepository : IDisposable
    {
        void Store(StoredEvent theEvent);
        IList<StoredEvent> All(Guid aggregateId);
    }
}