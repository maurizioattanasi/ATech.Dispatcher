using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace ATech.Dispatcher;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public async Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);

        var handler = serviceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(IRequestHandler<IRequest>.HandleAsync));

        var result = method!.Invoke(handler, [request, cancellationToken]);

        if (result is Task task)
            await task.ConfigureAwait(false);
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : notnull
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        var handler = serviceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.HandleAsync));

        var result = method!.Invoke(handler, [request, cancellationToken]);

        return result is Task<TResponse> task ? await task.ConfigureAwait(false) : (TResponse)result!;
    }
}