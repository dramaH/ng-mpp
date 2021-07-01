using ARchGLCloud.Domain.MPP.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.MPP.EventHandlers
{
    public class ProjectEventHandler : INotificationHandler<ProjectAddedEvent>, INotificationHandler<ProjectUpdatedEvent>, INotificationHandler<ProjectRemovedEvent>
    {
        public Task Handle(ProjectAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(ProjectUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(ProjectRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}