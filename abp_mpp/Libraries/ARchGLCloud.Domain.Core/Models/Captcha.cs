using System;

namespace ARchGLCloud.Domain.Core.Models
{
    public class Captcha : RedisAggregateRoot
    {
        public Captcha()
        {
        }

        public Captcha(Guid id) : base(id)
        {
        }

        public string Phone { get; set; }
        public string IP { get; set; }
        public string Subject { get; set; }
        public string Code { get; set; }
        public DateTimeOffset CreateTime { get; set; }
    }
}
