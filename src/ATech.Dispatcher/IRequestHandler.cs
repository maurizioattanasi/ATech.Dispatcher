using System.Threading.Tasks;

namespace ATech.Dispatcher;

public interface IRequestHandler<in TRequest>
    where TRequest : IRequest
{
    Task HandleAsync(TRequest request);
}

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    Task<TResponse> HandleAsync(TRequest request);
}
