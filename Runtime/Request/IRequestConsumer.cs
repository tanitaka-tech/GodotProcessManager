using System.Threading;
using Fractural.Tasks;

namespace TanitakaTech.GodotProcessManager
{
    public interface IRequestConsumer<TRequest>
    {
        public GDTask<TRequest> WaitRequestAndConsumeAsync(CancellationToken cancellationToken);
    }
}