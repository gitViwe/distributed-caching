using Microsoft.Extensions.Caching.Distributed;

namespace Shared;

public interface IRedisDistributedCache
{
    /// <summary>
    /// Gets a value with the given key.
    /// </summary>
    /// <typeparam name="TResult">The value type to return.</typeparam>
    /// <param name="key">A string identifying the requested value.</param>
    /// <returns>The located value or null.</returns>
    TResult? Get<TResult>(string key) where TResult : class, new();

    /// <summary>
    /// Gets a value with the given key.
    /// </summary>
    /// <param name="key">A string identifying the requested value.</param>
    /// <returns>The located value or null.</returns>
    string? Get(string key);

    /// <summary>
    /// Gets a value with the given key.
    /// </summary>
    /// <typeparam name="TResult">The value type to return.</typeparam>
    /// <param name="key">A string identifying the requested value.</param>
    /// <param name="token">Optional. The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the located value or null.</returns>
    Task<TResult?> GetAsync<TResult>(string key, CancellationToken token = default) where TResult : class, new();

    /// <summary>
    /// Gets a value with the given key.
    /// </summary>
    /// <param name="key">A string identifying the requested value.</param>
    /// <param name="token">Optional. The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the located value or null.</returns>
    Task<string?> GetAsync(string key, CancellationToken token = default);

    /// <summary>
    /// Sets a value with the given key.
    /// </summary>
    /// <typeparam name="TValue">The value type to store.</typeparam>
    /// <param name="key">A string identifying the requested value.</param>
    /// <param name="value">The value to set in the cache.</param>
    /// <param name="absoluteExpirationRelativeToNow">Gets or sets an absolute expiration time, relative to now.</param>
    /// <param name="slidingExpiration">
    /// Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
    /// This will not extend the entry lifetime beyond the absolute expiration (if set).
    /// </param>
    void Set<TValue>(string key, TValue value, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null);

    /// <summary>
    /// Sets the value with the given key.
    /// </summary>
    /// <typeparam name="TValue">The value type to store.</typeparam>
    /// <param name="key">A string identifying the requested value.</param>
    /// <param name="value">The value to set in the cache.</param>
    /// <param name="absoluteExpirationRelativeToNow">Gets or sets an absolute expiration time, relative to now.</param>
    /// <param name="slidingExpiration">
    /// Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
    /// This will not extend the entry lifetime beyond the absolute expiration (if set).
    /// </param>
    /// <param name="token">Optional. The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task SetAsync<TValue>(string key, TValue value, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null, CancellationToken token = default);

    /// <summary>
    /// Refreshes a value in the cache based on its key, resetting its sliding expiration timeout (if any).
    /// </summary>
    /// <param name="key">A string identifying the requested value.</param>
    void Refresh(string key);

    /// <summary>
    /// Refreshes a value in the cache based on its key, resetting its sliding expiration timeout (if any).
    /// </summary>
    /// <param name="key">A string identifying the requested value.</param>
    /// <param name="token">Optional. The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task RefreshAsync(string key, CancellationToken token = default);

    /// <summary>
    /// Removes the value with the given key.
    /// </summary>
    /// <param name="key">A string identifying the requested value.</param>
    void Remove(string key);

    /// <summary>
    /// Removes the value with the given key.
    /// </summary>
    /// <param name="key">A string identifying the requested value.</param>
    /// <param name="token">Optional. The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task RemoveAsync(string key, CancellationToken token = default);
}
