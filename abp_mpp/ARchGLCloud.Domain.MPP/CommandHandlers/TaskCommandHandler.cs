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
    public class TaskCommandHandler: CommandHandler, 
        IRequestHandler<AddTaskCommand>,
        IRequestHandler<UpdateTaskCommand>,
        IRequestHandler<RemoveTaskCommand>
    {
        private readonly ITaskRepository _repo;

        public TaskCommandHandler(ITaskRepository repo,
                                  IUnitOfWork uow,
                                  IMediatorHandler bus,
                                  INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repo = repo;
        }

        public Task<Unit> Handle(AddTaskCommand cmd, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }

        public Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Handle(RemoveTaskCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}