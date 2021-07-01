using ARchGLCloud.Domain.MPP.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.MPP.EventHandlers
{
    public class AssignmentEventHandler : INotificationHandler<AssignmentAddedEvent>, INotificationHandler<AssignmentUpdatedEvent>, INotificationHandler<AssignmentRemovedEvent>
    {
        public Task Handle(AssignmentAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(AssignmentUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(AssignmentRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}