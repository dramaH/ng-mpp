using ARchGLCloud.Application.Core.Interfaces;
using ARchGLCloud.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ARchGLCloud.Application.Core.Services
{
    public class AuthEmailMessageSender : IEmailSender
    {
        private readonly MailOptions _mailOptions;
        public AuthEmailMessageSender(MailOptions mailOptions)
        {
            this._mailOptions = mailOptions;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            EmailHelper.SendMail(_mailOptions, new List<string>() { email }, _mailOptions.Address , subject, message);
            return Task.FromResult(0);
        }
    }
}
