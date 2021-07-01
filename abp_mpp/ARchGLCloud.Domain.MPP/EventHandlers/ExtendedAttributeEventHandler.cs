using ARchGLCloud.Domain.MPP.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.MPP.EventHandlers
{
    public class ExtendedAttributeEventHandler : INotificationHandler<ExtendedAttributeAddedEvent>, INotificationHandler<ExtendedAttributeUpdatedEvent>, INotificationHandler<ExtendedAttributeRemovedEvent>
    {
        public Task Handle(ExtendedAttributeAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(ExtendedAttributeUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(ExtendedAttributeRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}