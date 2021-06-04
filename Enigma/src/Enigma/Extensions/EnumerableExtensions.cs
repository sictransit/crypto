using System;
using System.Collections.Generic;
using System.Linq;

namespace net.SicTransit.Crypto.Enigma.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            if (chunkSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(chunkSize));
            }

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value));
        }
    }
}
