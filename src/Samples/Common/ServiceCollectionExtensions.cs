using Microsoft.Extensions.DependencyInjection;

namespace Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSampleUtilitiesForBlazorServerSide(this IServiceCollection services, string? appName)
    {
        var databasePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DDev\\" + appName;

        services.AddSingleton<IKeyValueDb>(sp => new FileSystemKeyValueDb(databasePath));

        return services;
    }

    public static IServiceCollection AddSampleUtilitiesForBlazorWebAssemply(this IServiceCollection services, string? appName)
    {
        throw new NotImplementedException();
    }
}
