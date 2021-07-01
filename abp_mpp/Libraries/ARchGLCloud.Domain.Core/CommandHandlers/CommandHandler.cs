using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Commands;
using ARchGLCloud.Domain.Core.Notifications;
using ARchGLCloud.Domain.Core.Interfaces;
using MediatR;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.CommandHandlers
{
    public class CommandHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly DomainNotificationHandler _notifications;
        protected readonly IMediatorHandler _bus;

        public CommandHandler(IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications)
        {
            _uow = uow;
            _notifications = (DomainNotificationHandler)notifications;
            _bus = bus;
        }

        protected void NotifyValidationErrors(Command cmd)
        {
            foreach (var error in cmd.ValidationResult.Errors)
            {
                NotifyValidationErrors(cmd.MessageType, error.ErrorMessage);
            }
        }

        protected void NotifyValidationErrors(string key, string message)
        {
            _bus.RaiseEvent(new DomainNotification(key, message));
        }

        public bool Commit()
        {
            if (_notifications.HasNotifications()) return false;
            if (_uow.Commit()) return true;

            _bus.RaiseEvent(new DomainNotification("Commit", "We had a problem during saving your data."));
            return false;
        }

        public async Task<bool> CommitAsync()
        {
            if (_notifications.HasNotifications()) return false;

            bool success = await _uow.CommitAsync();
            if (success)
            {
                return true;
            }

            await _bus.RaiseEvent(new DomainNotification("Commit", "We had a problem during saving your data."));
            return false;
        }
    }
}