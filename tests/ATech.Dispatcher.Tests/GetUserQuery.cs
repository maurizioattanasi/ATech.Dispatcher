using System.Threading;
using System.Threading.Tasks;

namespace ATech.Dispatcher.Tests;

internal sealed record GetUserQuery(int UserId) : Request<UserResponse>;

internal sealed class GetUserHandler : IRequestHandler<GetUserQuery, UserResponse>
{
    public async Task<UserResponse> HandleAsync(GetUserQuery request, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1, cancellationToken).ConfigureAwait(false); // Minimal delay for async test
        return new UserResponse(request.UserId, "John Doe", "john@example.com");
    }
}
