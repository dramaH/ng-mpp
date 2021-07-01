using ARchGLCloud.Domain.MPP.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.MPP.EventHandlers
{
    public class CalendarWeekDayEventHandler : INotificationHandler<CalendarWeekDayAddedEvent>, INotificationHandler<CalendarWeekDayUpdatedEvent>, INotificationHandler<CalendarWeekDayRemovedEvent>
    {
        public Task Handle(CalendarWeekDayAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(CalendarWeekDayUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(CalendarWeekDayRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}