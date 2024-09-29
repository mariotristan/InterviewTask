using Microsoft.Extensions.Caching.Memory;

namespace Tests
{
    public class AuthorizationHandlerTest
    {
        private readonly IMemoryCache cache;

        public AuthorizationHandlerTest()
        {
            cache = new MemoryCache(new MemoryCacheOptions());
        }


        [Fact]
        public void HandleRequirementAsync_()
        {


        }
    }
}