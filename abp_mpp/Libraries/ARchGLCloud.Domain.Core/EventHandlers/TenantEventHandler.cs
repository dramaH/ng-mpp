using ARchGLCloud.Domain.Core.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.EventHandlers
{
    public class TenantEventHandler :
        INotificationHandler<TenantAddedEvent>,
        INotificationHandler<TenantUpdatedEvent>,
        INotificationHandler<TenantRemovedEvent>
    {
        public Task Handle(TenantAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TenantUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TenantRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}