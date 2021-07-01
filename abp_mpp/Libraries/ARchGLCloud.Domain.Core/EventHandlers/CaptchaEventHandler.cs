using ARchGLCloud.Domain.Core.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.EventHandlers
{
    public class CaptchaEventHandler : INotificationHandler<CaptchaAddedEvent>, INotificationHandler<CaptchaUpdatedEvent>, INotificationHandler<CaptchaRemovedEvent>
    {
        public Task Handle(CaptchaAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(CaptchaUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(CaptchaRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}