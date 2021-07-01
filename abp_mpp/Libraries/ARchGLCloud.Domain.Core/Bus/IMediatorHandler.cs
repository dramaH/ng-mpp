using System.Threading.Tasks;
using ARchGLCloud.Domain.Core.Commands;
using ARchGLCloud.Domain.Core.Events;


namespace ARchGLCloud.Domain.Core.Bus
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}
