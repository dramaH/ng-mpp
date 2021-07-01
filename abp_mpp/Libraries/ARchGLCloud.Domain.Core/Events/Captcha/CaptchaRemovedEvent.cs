using System;

namespace ARchGLCloud.Domain.Core.Events
{
    public class CaptchaRemovedEvent : Event
    {
        public CaptchaRemovedEvent()
        {
        }

        public CaptchaRemovedEvent(Guid id, string phone)
        {
            this.Id = this.AggregateId = Id;
            this.Phone = phone;
        }

        public Guid Id { get; set; }
        public string Phone { get; set; }
    }
}
