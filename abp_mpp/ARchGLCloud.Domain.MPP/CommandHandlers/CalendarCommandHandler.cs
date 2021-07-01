using System;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Notifications;
using ARchGLCloud.Domain.MPP.Interfaces;
using ARchGLCloud.Domain.MPP.Commands;
using ARchGLCloud.Domain.Core.Interfaces;
using ARchGLCloud.Domain.Core.CommandHandlers;

namespace ARchGLCloud.Domain.MPP.CommandHandlers
{
    public class CalendarCommandHandler: CommandHandler, 
        IRequestHandler<AddCalendarCommand>,
        IRequestHandler<UpdateCalendarCommand>,
        IRequestHandler<RemoveCalendarCommand>
    {
        private readonly ICalendarRepository _repo;

        public CalendarCommandHandler(ICalendarRepository repo,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repo = repo;
        }

        public Task<Unit> Handle(AddCalendarCommand cmd, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }

        public Task<Unit> Handle(UpdateCalendarCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Handle(RemoveCalendarCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}