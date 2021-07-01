using ARchGLCloud.Domain.MPP.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.MPP.EventHandlers
{
    public class CalendarExceptionEventHandler : INotificationHandler<CalendarExceptionAddedEvent>, INotificationHandler<CalendarExceptionUpdatedEvent>, INotificationHandler<CalendarExceptionRemovedEvent>
    {
        public Task Handle(CalendarExceptionAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(CalendarExceptionUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(CalendarExceptionRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}