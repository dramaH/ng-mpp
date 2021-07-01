using ARchGLCloud.Domain.Core.Events;
using IdentityServer4.Events;
using IdentityServer4.Services;
using System;
using System.Threading.Tasks;

namespace ARchGLCloud.Application.Core.Services
{
    public class IdentityEventService : IEventService
    {
        IEventStore eventStore;
        public IdentityEventService(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public bool CanRaiseEventType(EventTypes evtType)
        {
            return true;
        }

        public Task RaiseAsync(IdentityServer4.Events.Event @event)
        {
            try
            {
                var se = new LoginEvent()
                {
                    ActivityId = @event.ActivityId,
                    Category = @event.Category,
                    Id = @event.Id,
                    LocalIpAddress = @event.LocalIpAddress,
                    Message = @event.Message,
                    Name = @event.Name,
                    ProcessId = @event.ProcessId,
                    RemoteIpAddress = @event.RemoteIpAddress
                };

                if (@event is UserLoginSuccessEvent)
                {
                    var ulfe = (UserLoginSuccessEvent)@event;
                    se.UserName = ulfe.Username;
                    se.RemoteIpAddress = ulfe.RemoteIpAddress;
                }
                else if(@event is UserLoginFailureEvent)
                {
                    var ulfe = (UserLoginFailureEvent)@event;
                    se.UserName = ulfe.Username;
                }
                else
                {
                    se.UserName = @event.Name;
                }

                this.eventStore.Save(se);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }
    }

    public class LoginEvent : Domain.Core.Events.Event
    {
        public LoginEvent()
        {
            this.AggregateId = Guid.NewGuid();
        }

        public string Category { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        public string ActivityId { get; set; }
        public int ProcessId { get; set; }
        public string LocalIpAddress { get; set; }
        public string RemoteIpAddress { get; set; }
    }
}
