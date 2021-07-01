using ARchGLCloud.Domain.MPP.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.MPP.EventHandlers
{
    public class TaskBaselineEventHandler : INotificationHandler<TaskBaselineAddedEvent>, INotificationHandler<TaskBaselineUpdatedEvent>, INotificationHandler<TaskBaselineRemovedEvent>
    {
        public Task Handle(TaskBaselineAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TaskBaselineUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TaskBaselineRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}