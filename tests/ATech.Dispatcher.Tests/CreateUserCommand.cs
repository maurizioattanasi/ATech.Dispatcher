using System.Threading;
using System.Threading.Tasks;

namespace ATech.Dispatcher.Tests;

internal sealed record CreateUserCommand(string Name, string Email) : Request<int>;

internal sealed class CreateUserHandler() : IRequestHandler<CreateUserCommand, int>
{
    public async Task<int> HandleAsync(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult<int>(123).ConfigureAwait(false);
    }
}
