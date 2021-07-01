using ARchGLCloud.Domain.Core.Events;
using ARchGLCloud.Domain.Core.Interfaces;
using ARchGLCloud.Domain.Core.Repositories;
using Newtonsoft.Json;

namespace ARchGLCloud.Infra.Core.EventSourcing
{
    public class SqlEventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IUser _user;

        public SqlEventStore(IEventStoreRepository eventStoreRepository, IUser user)
        {
            _eventStoreRepository = eventStoreRepository;
            _user = user;
        }

        public void Save<T>(T @event) where T : Event
        {
            var serializedData = JsonConvert.SerializeObject(@event);

            var storedEvent = new StoredEvent(
                @event,
                serializedData,
                _user.Name);

            _eventStoreRepository.Store(storedEvent);
        }
    }
}