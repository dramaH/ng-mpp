using System;
using MediatR;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Notifications;
using ARchGLCloud.Domain.Core.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ARchGLCloud.Domain.Core.Repositories;
using ARchGLCloud.Domain.Core.Models;
using ARchGLCloud.Domain.Core.Commands;
using ARchGLCloud.Domain.Core.Events;

namespace ARchGLCloud.Domain.Core.CommandHandlers
{
    public class TenantCommandHandler : CommandHandler,
        IRequestHandler<AddTenantCommand>,
        IRequestHandler<UpdateTenantCommand>,
        IRequestHandler<RemoveTenantCommand>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMediatorHandler Bus;

        public TenantCommandHandler(ITenantRepository tenantRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _tenantRepository = tenantRepository;
            Bus = bus;
        }

        public Task<Unit> Handle(AddTenantCommand cmd, CancellationToken cancellationToken)
        {
            if (!cmd.IsValid())
            {
                NotifyValidationErrors(cmd);
                return Unit.Task;
            }

            var tenant = new Tenant(cmd.Id, Guid.NewGuid());
            tenant.TenantId = cmd.TenantId;
            tenant.EName = cmd.EName;
            tenant.Name = cmd.Name;
            tenant.Host = cmd.Host;
            tenant.ConnectionString = cmd.ConnectionString;
            tenant.Enabled = cmd.Enabled;

            _tenantRepository.Add(tenant);

            if (Commit())
            {
                Bus.RaiseEvent(new TenantAddedEvent(tenant.Id, tenant.TenantId, tenant.EName, tenant.Name, tenant.Host, tenant.ConnectionString, tenant.Enabled));
            }

            return Unit.Task;
        }

        public Task<Unit> Handle(UpdateTenantCommand cmd, CancellationToken cancellationToken)
        {
            if (!cmd.IsValid())
            {
                NotifyValidationErrors(cmd);
                return Unit.Task;
            }


            var tenant = _tenantRepository.Find(cmd.Id);

            if (tenant != null)
            {
                tenant.TenantId = cmd.TenantId;
                tenant.EName = cmd.EName;
                tenant.Name = cmd.Name;
                tenant.Host = cmd.Host;
                tenant.ConnectionString = cmd.ConnectionString;
                tenant.Enabled = cmd.Enabled;
            }

            _tenantRepository.Update(tenant);

            if (Commit())
            {
                Bus.RaiseEvent(new TenantUpdatedEvent(tenant.Id, tenant.TenantId, tenant.EName, tenant.Name, tenant.Host, tenant.ConnectionString, tenant.Enabled));
            }

            return Unit.Task;
        }

        public Task<Unit> Handle(RemoveTenantCommand cmd, CancellationToken cancellationToken)
        {
            if (!cmd.IsValid())
            {
                NotifyValidationErrors(cmd);
                return Unit.Task;
            }

            _tenantRepository.Delete(cmd.Id);

            if (Commit())
            {
                Bus.RaiseEvent(new TenantRemovedEvent(cmd.Id, cmd.TenantId));
            }

            return Unit.Task;
        }

        public void Dispose()
        {
            _tenantRepository.Dispose();
        }
    }
}