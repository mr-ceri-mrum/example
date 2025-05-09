using StackExchange.Redis;

namespace HomeDelivery.Order.Business.Services;

public interface IRedisService
{
    Task SetAsync(string key, string value, TimeSpan expiration);
    Task UpdateStatus(string key, string value, TimeSpan expiration);
    Task<string?> GetAsync(string key);
    Task DeleteAsync(string key);
}

public class RedisService : IRedisService
{
    private readonly IDatabase _database;

    public RedisService(IConnectionMultiplexer connectionMultiplexer, IDatabase database)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task SetAsync(string key, string value, TimeSpan expiration)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        ArgumentNullException.ThrowIfNull(value);

        await _database.StringSetAsync(key, value, expiration);
    }

    public async Task UpdateStatus(string key, string value, TimeSpan expiration)
    {
        var entity = await GetAsync(key);
        if (entity != null) 
            await DeleteAsync(entity);
        await SetAsync(key, value, expiration);
    }

    public async Task<string?> GetAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        var value = await _database.StringGetAsync(key);
        return value.HasValue ? value.ToString() : null;
    }

    public async Task DeleteAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        await _database.KeyDeleteAsync(key);
    }
}