using System;

namespace ARchGLCloud.Domain.Core.Events
{
    public class CaptchaEvent : Event
    {
        public CaptchaEvent()
        {
        }

        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string IP { get; set; }
        public string Subject { get; set; }
        public string Code { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public int Expiration { get; set; }
    }
}
