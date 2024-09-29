using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Web;
using System.Collections.Concurrent;

namespace RNGService.Services
{
    public class UserStatisticsService : IUserStatisticsService, IDisposable
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentBag<string> _users;

        public UserStatisticsService(IMemoryCache cache)
        {
            this._users = new ConcurrentBag<string>();
            this._cache = cache;
        }

        public Task CacheUserStatistics(IHttpContextAccessor context)
        {
            var cacheKey = context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimConstants.ObjectId);
            foreach (var user in _users)
            {
                // adds user key if collection does not contain, otherwise it already contains
                if (user == cacheKey!.Type) continue;
                _users.Add(cacheKey!.Type);
            }
            _cache.TryGetValue<int>(cacheKey!, out var userStatistics);
            return Task.FromResult(_cache.Set(cacheKey!, userStatistics++, DateTimeOffset.UtcNow.AddMinutes(10)));
        }

        public ValueTask<int> GetUserStatistics(IHttpContextAccessor context)
        {
            var cacheKey = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimConstants.ObjectId);
            return new ValueTask<int>(_cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var expiration = DateTimeOffset.UtcNow.AddMinutes(10);
                entry.AbsoluteExpiration = expiration;
                return 0;
            }));
        }

        public void Dispose()
        {
            if (!_users.IsEmpty)
            {
                _users.Clear();
            }
            if (_cache != null)
            {
                _cache.Dispose();
            }
        }
    }
}
