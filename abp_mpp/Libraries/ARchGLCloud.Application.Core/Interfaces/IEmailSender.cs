using System.Threading.Tasks;

namespace ARchGLCloud.Application.Core.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
