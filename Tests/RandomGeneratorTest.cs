using System;
using System.Collections.Generic;
using System.Linq; // Add this using directive
using Microsoft.AspNetCore.Http;
using Moq;
using RNGService.Services;
using Xunit;

namespace RNGService.Tests
{
    public class RandomGeneratorTests : IDisposable
    {
        private readonly RandomGenerator _randomGenerator;
        private readonly Mock<IUserStatisticsService> _userStatisticsServiceMock;
        private readonly Mock<IHttpContextAccessor> _contextAccessorMock;

        public RandomGeneratorTests()
        {
            _userStatisticsServiceMock = new Mock<IUserStatisticsService>();
            _contextAccessorMock = new Mock<IHttpContextAccessor>();
            _randomGenerator = new RandomGenerator(_userStatisticsServiceMock.Object, _contextAccessorMock.Object);
        }

        [Fact]
        public void GetBytes_ShouldThrowArgumentOutOfRangeException_WhenLenIsZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _randomGenerator.GetBytes(0));
        }

        [Fact]
        public void GetBytes_ShouldThrowArgumentOutOfRangeException_WhenLenIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _randomGenerator.GetBytes(-1));
        }

        [Fact]
        public void GetBytes_ShouldThrowArgumentOutOfRangeException_WhenLenIsTooLarge()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _randomGenerator.GetBytes(1 << 16));
        }

        [Fact]
        public void GetBytes_ShouldReturnCorrectLength()
        {
            int len = 10;
            var result = _randomGenerator.GetBytes(len);
            Assert.Equal(len, result.Count()); // Use Count() instead of Length
        }

        [Fact]
        public void GetBytes_ShouldReturnUniqueValues()
        {
            int len = 10;
            var result1 = _randomGenerator.GetBytes(len);
            var result2 = _randomGenerator.GetBytes(len);
            Assert.NotEqual(result1, result2);
        }

        public void Dispose()
        {
            _randomGenerator.Dispose();
        }
    }
}
