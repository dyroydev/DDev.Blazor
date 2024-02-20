using DDev.Blazor.Components;
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
    /// <item><see cref="ILocalStorage"/> (Scoped)</item>
    /// <item><see cref="ISessionStorage"/> (Scoped)</item>
    /// <item><see cref="IMessageBox"/> (Scoped)</item>
    /// <item><see cref="IKeyBindingsFactory"/> (Scoped)</item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddDDevBlazor(this IServiceCollection services)
    {
        return services
            .AddScoped<PortalRegistry>()
            .AddScoped<BackdropStack>()
            .AddScoped<IKeyBindingsFactory, KeyBindingsFactory>()
            .AddScoped<IMessageBox, MessageBox>()
            .AddScoped<ILocalStorage, LocalStorage>()
            .AddScoped<ISessionStorage, SessionStorage>()
            .AddScoped<TaskCompletionSource<MessageBoxProvider>>()
            .AddKeyedScoped<TaskCompletionSource>(nameof(DDevBlazor));
    }
}