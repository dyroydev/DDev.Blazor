using System.Text.Json;

namespace ScheduleApp.Services;

public interface IStorage
{
    Task PutAsync<T>(string key, T value, CancellationToken cancellationToken = default);

    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    Task DeleteAsync<T>(string key, CancellationToken cancellationToken = default);

    Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken = default);
}

public class LocalFileSystemStorage : IStorage
{
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
    };

    private readonly string _path;

    public LocalFileSystemStorage(string path)
    {
        _path = path;
    }

    public async Task DeleteAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var table = await GetAllInternal<T>(cancellationToken).ConfigureAwait(false);
        table.Remove(key);
        await PutAllInternal(table, cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken = default)
    {
        var table = await GetAllInternal<T>(cancellationToken).ConfigureAwait(false);
        return table.Values.ToList();
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var table = await GetAllInternal<T>(cancellationToken).ConfigureAwait(false);
        return table.GetValueOrDefault(key);
    }

    public async Task PutAsync<T>(string key, T value, CancellationToken cancellationToken = default)
    {
        var table = await GetAllInternal<T>(cancellationToken).ConfigureAwait(false);
        table[key] = value;
        await PutAllInternal(table, cancellationToken).ConfigureAwait(false);
    }

    private async Task<Dictionary<string, T>> GetAllInternal<T>(CancellationToken cancellationToken)
    {
        var path = $"{_path}\\{typeof(T).Name}.json";
        if (!File.Exists(path))
            return new Dictionary<string, T>();

        var json = await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);

        return JsonSerializer.Deserialize<Dictionary<string, T>>(json, _jsonOptions)!;
    }

    private async Task PutAllInternal<T>(Dictionary<string, T> values, CancellationToken cancellationToken)
    {
        var path = $"{_path}\\{typeof(T).Name}.json";

        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);

        var json = JsonSerializer.Serialize(values, _jsonOptions);
        await File.WriteAllTextAsync(path, json, cancellationToken).ConfigureAwait(false);
    }
}