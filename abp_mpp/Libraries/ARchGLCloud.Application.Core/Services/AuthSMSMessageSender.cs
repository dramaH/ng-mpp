using ARchGLCloud.Application.Core.Interfaces;
using ARchGLCloud.Application.Core.ViewModels;
using ARchGLCloud.Core;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Notifications;
using System.Threading.Tasks;

namespace ARchGLCloud.Application.Core.Services
{
    public class AuthSMSMessageSender : ISmsSender
    {
        private readonly ICaptchaService _captchaService;
        private readonly IMediatorHandler _bus;
        public AuthSMSMessageSender(ICaptchaService captchaService, IMediatorHandler bus)
        {
            this._captchaService = captchaService;
            this._bus = bus;
        }

        public async Task<bool> SendSmsAsync(CaptchaViewModel captcha)
        {
            await _captchaService.AddAsync(captcha);
            /*
            var captchaList = await _captchaService.GetEntities(captcha.IP, 0, 2);
            if (captchaList.Count == 2)
            {
                var first = captchaList[0];
                var last = captchaList[1];

                if ((last.CreateTime - first.CreateTime).TotalSeconds < 59)
                {
                    await this._bus.RaiseEvent(new DomainNotification("SENDFAILED", "发送失败"));
                    return false;
                }
            }*/ 

            SMSHelper.SendSMS(captcha.Phone, captcha.Code, captcha.SignName ?? "筑智建", captcha.TemplateCode ?? "SMS_176943086");
            return true;
        }
    }
}
