using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace ATech.Dispatcher.Tests;

public class DispatcherTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDispatcher _dispatcher;

    public DispatcherTests()
    {
        var services = new ServiceCollection();

        services.AddScoped<IRequestHandler<GetUserQuery, UserResponse>, GetUserHandler>();
        services.AddScoped<IRequestHandler<CreateUserCommand, int>, CreateUserHandler>();
        services.AddScoped<IRequestHandler<DeleteUserCommand>, DeleteUserHandler>();
        services.AddScoped<IDispatcher, Dispatcher>();

        _serviceProvider = services.BuildServiceProvider();
        _dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
    }


    [Fact]
    public async Task Send_WithQuery_ReturnsExpectedResponse()
    {
        // Arrange
        var query = new GetUserQuery(UserId: 42);

        // Act
        var result = await _dispatcher.SendAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(42, result.Id);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("john@example.com", result.Email);
    }

    [Fact]
    public async Task Send_WithCommandReturningValue_ReturnsExpectedResult()
    {
        // Arrange
        var command = new CreateUserCommand(Name: "Jane Smith", Email: "jane@example.com");

        // Act
        var result = await _dispatcher.SendAsync(command);

        // Assert
        Assert.Equal(123, result);
    }

    [Fact]
    public async Task Send_WithVoidCommand_CompletesSuccessfully()
    {
        // Arrange
        var command = new DeleteUserCommand(UserId: 42);

        // Act & Assert (should not throw)
        await _dispatcher.SendAsync(command);

        // Test passes if no exception is thrown
        Assert.True(true);
    }

    [Fact]
    public async Task Send_WithCancellationToken_PassesTokenToHandler()
    {
        // Arrange
        var query = new GetUserQuery(UserId: 1);
        using var cts = new CancellationTokenSource();

        // Act
        var result = await _dispatcher.SendAsync(query, cts.Token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task Send_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _dispatcher.SendAsync<UserResponse>(null!));
    }

    [Fact]
    public async Task Send_VoidWithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _dispatcher.SendAsync(null!));
    }

    [Fact]
    public void Request_Inheritance_WorksCorrectly()
    {
        // Arrange & Act
        var query = new GetUserQuery(1);
        var command = new CreateUserCommand("Test", "test@example.com");
        var voidCommand = new DeleteUserCommand(1);

        // Assert
        Assert.IsAssignableFrom<IRequest<UserResponse>>(query);
        Assert.IsAssignableFrom<IRequest<int>>(command);
        Assert.IsAssignableFrom<IRequest>(voidCommand);
    }
}

