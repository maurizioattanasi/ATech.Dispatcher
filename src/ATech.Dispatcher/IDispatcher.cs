using System.Threading;
using System.Threading.Tasks;

namespace ATech.Dispatcher;

public interface IDispatcher
{
    Task SendAsync(IRequest request, CancellationToken cancellationToken = default);

    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : notnull;
}
