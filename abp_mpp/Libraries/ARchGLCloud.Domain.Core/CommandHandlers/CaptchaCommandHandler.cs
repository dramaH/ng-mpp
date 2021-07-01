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
    public class CaptchaCommandHandler : CommandHandler,
        IRequestHandler<AddCaptchaCommand>,
        IRequestHandler<UpdateCaptchaCommand>,
        IRequestHandler<RemoveCaptchaCommand>
    {
        private readonly ICaptchaRepository _captchaRepository;

        public CaptchaCommandHandler(ICaptchaRepository captchaRepository,
                                     IUnitOfWork uow,
                                     IMediatorHandler bus,
                                     INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _captchaRepository = captchaRepository;
        }

        public async Task<Unit> Handle(AddCaptchaCommand cmd, CancellationToken cancellationToken)
        {
            if (!cmd.IsValid())
            {
                NotifyValidationErrors(cmd);
                return Unit.Value;
            }

            var captcha = new Captcha(cmd.Id);
            captcha.Code = cmd.Code;
            captcha.CreateTime = cmd.CreateTime;
            captcha.Expiration = cmd.Expiration;
            captcha.Phone = cmd.Phone;
            captcha.IP = cmd.IP;
            captcha.Subject = cmd.Subject;

            await _captchaRepository.AddAsync(captcha);

            if (true)
            {
                var cae = new CaptchaAddedEvent()
                {
                    Code = captcha.Code,
                    CreateTime = captcha.CreateTime,
                    Expiration = captcha.Expiration,
                    Phone = captcha.Phone,
                    IP = captcha.IP,
                    Subject = captcha.Subject
                };

                await _bus.RaiseEvent(cae);
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateCaptchaCommand cmd, CancellationToken cancellationToken)
        {
            if (!cmd.IsValid())
            {
                NotifyValidationErrors(cmd);
                return Unit.Value;
            }

            var captcha = await _captchaRepository.GetAsync(cmd.Phone);

            if (captcha != null)
            {
                captcha.Code = cmd.Code;
                captcha.CreateTime = cmd.CreateTime;
                captcha.Expiration = cmd.Expiration;
                captcha.Phone = cmd.Phone;
                captcha.IP = cmd.IP;
                captcha.Subject = cmd.Subject;
            }

            await _captchaRepository.UpdateAsync(captcha.Phone, captcha);

            if (true)
            {
                var cae = new CaptchaAddedEvent()
                {
                    Code = captcha.Code,
                    CreateTime = captcha.CreateTime,
                    Expiration = captcha.Expiration,
                    Phone = captcha.Phone,
                    IP = captcha.IP,
                    Subject = captcha.Subject
                };

                await _bus.RaiseEvent(cae);
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(RemoveCaptchaCommand cmd, CancellationToken cancellationToken)
        {
            if (!cmd.IsValid())
            {
                NotifyValidationErrors(cmd);
                return Unit.Value;
            }

            await _captchaRepository.RemoveAsync(cmd.Phone);

            if (true)
            {
                await _bus.RaiseEvent(new CaptchaRemovedEvent(cmd.Id, cmd.Phone));
            }

            return Unit.Value;
        }

        public void Dispose()
        {
            _captchaRepository.Dispose();
        }
    }
}