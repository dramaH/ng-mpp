using ARchGLCloud.Domain.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.Repositories
{
    public interface ICaptchaRepository : IRedisStore<Captcha>
    {
        Task<List<string>> GetEntities(string key, int start, int stop);
    }
}
