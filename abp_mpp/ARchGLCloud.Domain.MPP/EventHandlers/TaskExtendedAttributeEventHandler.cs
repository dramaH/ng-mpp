using ARchGLCloud.Domain.MPP.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.MPP.EventHandlers
{
    public class TaskExtendedAttributeEventHandler : INotificationHandler<TaskExtendedAttributeAddedEvent>, INotificationHandler<TaskExtendedAttributeUpdatedEvent>, INotificationHandler<TaskExtendedAttributeRemovedEvent>
    {
        public Task Handle(TaskExtendedAttributeAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TaskExtendedAttributeUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TaskExtendedAttributeRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}