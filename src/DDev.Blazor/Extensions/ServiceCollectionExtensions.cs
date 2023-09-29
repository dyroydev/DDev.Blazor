using DDev.Blazor.Internal;
using DDev.Blazor.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DDev.Blazor.Extensions;

/// <summary>
/// Extensions required to add and configure services for DDev.Blazor.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add and configure services required for DDev.Blazor.
    /// </summary>
    /// <remarks>
    /// The following services will be available:
    /// <list type="bullet">
    /// <item><see cref="IMessageBox"/> (Scoped)</item>
    /// <item><see cref="IKeyBindingsFactory"/> (Scoped)</item>
    /// <item><see cref="IKeyBindings"/> (Transient, must be disposed after use)</item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddDDevBlazor(this IServiceCollection services)
    {
        return services
            .AddScoped<PortalRegistry>()
            .AddScoped<DDevTools>()
            .AddScoped<BackdropStack>()
            .AddScoped<IKeyBindingsFactory, KeyBindingsFactoryInternal>()
            .AddTransient<IKeyBindings>(sp => sp.GetRequiredService<IKeyBindingsFactory>().ForDocument())
            .AddDDevBlazorTool<IMessageBox, MessageBoxInternal>();
    }

    /// <summary>
    /// Adds a internal tool to the service collection. The tool must be made available from within <see cref="Components.DDevBlazor"/>.
    /// </summary>
    private static IServiceCollection AddDDevBlazorTool<TService, TImplementation>(this IServiceCollection services) where TImplementation : TService where TService : class
    {
        return services.AddScoped<TService>(sp => sp.GetRequiredService<DDevTools>().Get<TImplementation>());
    }
}