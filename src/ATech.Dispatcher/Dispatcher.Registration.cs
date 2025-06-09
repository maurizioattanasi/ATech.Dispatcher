using System;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ATech.Dispatcher;

public static partial class DispatcherRegistration
{
    /// <summary>
    /// Registers the dispatcher services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddDispatcher(this IServiceCollection services,
                                                   Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assembly);

        // Register the dispatcher
        services.AddScoped<IDispatcher, Dispatcher>();

        // Register IRequestHandler<,>
        ServiceDescriptor[] serviceDescriptorsGeneric = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IRequestHandler<,>)))
            .Select(type => ServiceDescriptor.Scoped(typeof(IRequestHandler<,>), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptorsGeneric);

        // Register IRequestHandler<>
        ServiceDescriptor[] serviceDescriptorsSingle = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IRequestHandler<>)))
            .Select(type => ServiceDescriptor.Scoped(typeof(IRequestHandler<>), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptorsSingle);

        // // Use Scrutor to register all handlers
        // services.Scan(scan => scan
        //     .FromAssemblies(assemblies.Length > 0 ? assemblies : [Assembly.GetCallingAssembly()])
        //     .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
        //     .AsImplementedInterfaces()
        //     .WithScopedLifetime());

        // services.Scan(scan => scan
        //     .FromAssemblies(assemblies.Length > 0 ? assemblies : [Assembly.GetCallingAssembly()])
        //     .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
        //     .AsImplementedInterfaces()
        //     .WithScopedLifetime());

        return services;
    }
}