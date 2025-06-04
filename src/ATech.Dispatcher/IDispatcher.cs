using Microsoft.Extensions.DependencyInjection;

namespace ATech.Dispatcher;

public interface IDispatcher
{
    Task SendAsync(IRequest request, CancellationToken cancellationToken = default);

    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : notnull;
}

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("This method is not implemented. Use SendAsync<TResponse> instead.");
    }

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : notnull
    {
        throw new NotImplementedException("This method is not implemented. Use SendAsync<TResponse> instead.");
    }
}