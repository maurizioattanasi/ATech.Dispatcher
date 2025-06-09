using System.Threading;
using System.Threading.Tasks;

namespace ATech.Dispatcher.Tests;

internal sealed record DeleteUserCommand(int UserId) : Request;

internal sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    public async Task HandleAsync(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken).ConfigureAwait(false);
    }
}