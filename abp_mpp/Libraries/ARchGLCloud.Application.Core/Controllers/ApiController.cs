using ARchGLCloud.Domain.Core.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ARchGLCloud.Domain.Core.Bus;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using IdentityModel;

namespace ARchGLCloud.Application.Core.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;

        public Guid UserId
        {
            get
            {
                return Guid.Parse(User.Claims.FirstOrDefault(uc => uc.Type == JwtClaimTypes.Subject)?.Value);
            }
        }

        public string ClientId
        {
            get
            {
                return User.Claims.FirstOrDefault(x => x.Type == "client_id")?.Value;
            }
        }

        public UserClaimsViewModel CurrentUser
        {
            get
            {
                Guid.TryParse(User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value, out Guid userId);
                Guid.TryParse(User.Claims.FirstOrDefault(x => x.Type == "company_id")?.Value, out Guid companyId);
                var CurrentUser = new UserClaimsViewModel
                {
                    Id = userId,
                    CompanyId = companyId,
                    CompanyName = User.Claims.FirstOrDefault(x => x.Type == "company_name")?.Value,
                    Name = User.Claims.FirstOrDefault(x => x.Type == "name")?.Value,
                    Role = User.Claims.Where(x => x.Type == "role").Select(x => x.Value).ToList()
                };
                return CurrentUser;
            }
        }

        protected ApiController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediator = mediator;
        }

        protected IEnumerable<DomainNotification> Notifications => _notifications.GetNotifications();

        protected bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }

        protected new IActionResult Response(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(result);
            }

            return BadRequest(new
            {
                total = 1,
                success = false,
                items = _notifications.GetNotifications().Select(n => n.Value)
            });
        }

        protected void NotifyModelStateErrors()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        protected void NotifyError(string code, string message)
        {
            _mediator.RaiseEvent(new DomainNotification(code, message));
        }

        protected void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotifyError(result.ToString(), error.Description);
            }
        }
    }
}
