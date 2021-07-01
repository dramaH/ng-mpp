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
    public class CalendarExceptionCommandHandler: CommandHandler, 
        IRequestHandler<AddCalendarExceptionCommand>,
        IRequestHandler<UpdateCalendarExceptionCommand>,
        IRequestHandler<RemoveCalendarExceptionCommand>
    {
        private readonly ICalendarExceptionRepository _repo;

        public CalendarExceptionCommandHandler(ICalendarExceptionRepository repo,
                                               IUnitOfWork uow,
                                               IMediatorHandler bus,
                                               INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repo = repo;
        }

        public Task<Unit> Handle(AddCalendarExceptionCommand cmd, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }

        public Task<Unit> Handle(UpdateCalendarExceptionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Handle(RemoveCalendarExceptionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}