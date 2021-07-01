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
    public class ExtendedAttributeCommandHandler : CommandHandler,
        IRequestHandler<AddExtendedAttributeCommand>,
        IRequestHandler<UpdateExtendedAttributeCommand>,
        IRequestHandler<RemoveExtendedAttributeCommand>
    {
        private readonly IExtendedAttributeRepository _repo;

        public ExtendedAttributeCommandHandler(IExtendedAttributeRepository repo,
                                               IUnitOfWork uow,
                                               IMediatorHandler bus,
                                               INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repo = repo;
        }

        public Task<Unit> Handle(AddExtendedAttributeCommand cmd, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }

        public Task<Unit> Handle(UpdateExtendedAttributeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Handle(RemoveExtendedAttributeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}