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
            .AddScoped<BackdropStack>()
            .AddScoped<IKeyBindingsFactory, KeyBindingsFactoryInternal>()
            .AddScoped<IMessageBox, MessageBox>()
            .AddScoped<TaskCompletionSource<MessageBoxComponent>>();
    }
}