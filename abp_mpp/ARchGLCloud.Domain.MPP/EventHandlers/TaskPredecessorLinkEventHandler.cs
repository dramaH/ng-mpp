using ARchGLCloud.Domain.MPP.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.MPP.EventHandlers
{
    public class TaskPredecessorLinkEventHandler : INotificationHandler<TaskPredecessorLinkAddedEvent>, INotificationHandler<TaskPredecessorLinkUpdatedEvent>, INotificationHandler<TaskPredecessorLinkRemovedEvent>
    {
        public Task Handle(TaskPredecessorLinkAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TaskPredecessorLinkUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TaskPredecessorLinkRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}