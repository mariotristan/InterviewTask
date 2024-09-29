namespace RNGService.Services
{
    public interface IUserStatisticsService
    {
        public Task CacheUserStatistics(IHttpContextAccessor context);
        public ValueTask<int> GetUserStatistics(IHttpContextAccessor context);
    }
}