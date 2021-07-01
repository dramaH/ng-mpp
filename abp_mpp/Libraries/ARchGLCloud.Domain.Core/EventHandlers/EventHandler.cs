using ARchGLCloud.Domain.Core.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.SSO.EventHandlers
{
    public class EventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : Event
    {
        public Task Handle(TEvent @event, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}