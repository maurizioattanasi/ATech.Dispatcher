namespace ATech.Dispatcher;

public abstract record Request : IRequest;

public abstract record Request<TResponse> : IRequest<TResponse>
    where TResponse : notnull;
