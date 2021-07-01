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
    public class AssignmentCommandHandler: CommandHandler, 
        IRequestHandler<AddAssignmentCommand>,
        IRequestHandler<UpdateAssignmentCommand>,
        IRequestHandler<RemoveAssignmentCommand>
    {
        private readonly IAssignmentRepository _repo;

        public AssignmentCommandHandler(IAssignmentRepository repo,
                                        IUnitOfWork uow,
                                        IMediatorHandler bus,
                                        INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repo = repo;
        }

        public Task<Unit> Handle(AddAssignmentCommand cmd, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }

        public Task<Unit> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Handle(RemoveAssignmentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}