using ARchGLCloud.Application.Core.ViewModels;
using System.Threading.Tasks;

namespace ARchGLCloud.Application.Core.Interfaces
{
    public interface ISmsSender
    {
        Task<bool> SendSmsAsync(CaptchaViewModel captcha);
    }
}
