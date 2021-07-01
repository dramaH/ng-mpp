using System;
using System.Linq;
using System.Collections.Generic;
using ARchGLCloud.Domain.Core.Repositories;
using ARchGLCloud.Domain.Core.Events;
using Microsoft.EntityFrameworkCore;

namespace ARchGLCloud.Infra.Core.Repositories
{
    public class EventStoreSQLRepository<TDbContext> : IEventStoreRepository where TDbContext : DbContext
    {
        private readonly TDbContext _context;

        public EventStoreSQLRepository(TDbContext context)
        {
            _context = context;
        }

        public IList<StoredEvent> All(Guid aggregateId)
        {
            return (from e in _context.Set<StoredEvent>() where e.AggregateId == aggregateId select e).ToList();
        }

        public void Store(StoredEvent @event)
        {
            _context.Set<StoredEvent>().Add(@event);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}