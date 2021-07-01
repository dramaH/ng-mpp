using ARchGLCloud.Application.Core.Filters;
using ARchGLCloud.Application.Core.ViewModels;
using System;

namespace ARchGLCloud.Application.Core.Interfaces
{
    public interface ICaptchaService : ICacheService<Guid, CaptchaQueryFilter, CaptchaViewModel>
    {
    }
}
