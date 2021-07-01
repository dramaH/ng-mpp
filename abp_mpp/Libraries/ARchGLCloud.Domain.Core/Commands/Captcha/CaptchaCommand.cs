using System;

namespace ARchGLCloud.Domain.Core.Commands
{
    public abstract class CaptchaCommand : Command
    {
        public CaptchaCommand()
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
