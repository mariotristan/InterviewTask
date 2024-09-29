using System;

namespace RNGService.Services {
    public interface IRandomGenerator {
        IEnumerable<byte> GetBytes(int len);
    }

}