using System;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
        Task<bool> CommitAsync();
    }
}
