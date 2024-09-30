namespace RNGService.Services
{
    public sealed class RandomGenerator : IRandomGenerator, IDisposable
    {
        Random _rng = new Random();
        object _mutex = new Object();
        private readonly IUserStatisticsService _userStatisticsService;
        private readonly IHttpContextAccessor _context;

        public RandomGenerator(IUserStatisticsService userStatisticsService, IHttpContextAccessor context)
        {
            this._userStatisticsService = userStatisticsService;
            this._context = context;
        }

        public IEnumerable<byte> GetBytes(int len)
        {
            if (len <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(len), $"Must request a postive amount of bytes");
            }
            if (len >= 1 << 16)
            {
                throw new ArgumentOutOfRangeException(nameof(len), $"Must be less than 2^16");
            }

            var bytes = new byte[len];
            lock (_mutex)
            {
                _rng.NextBytes(bytes);
            }
            // _userStatisticsService.CacheUserStatistics(_context).Wait();

            return bytes;
        }

        public void Dispose()
        {
        }
    }
}