using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IResponseCacheRepository
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}