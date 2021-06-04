using System.Linq;

namespace net.SicTransit.Crypto.Enigma.Extensions
{
    public static class TextExtensions
    {
        public static char WrapAround(this char c)
        {
            return c switch
            {
                < 'A' => (char)(c + 26),
                > 'Z' => (char)(c - 26),
                _ => c
            };
        }

        public static string ToEnigmaText(this string s)
        {
            return new(s.ToUpperInvariant().Where(c => c >= 'A' && c <= 'Z').ToArray());
        }

        public static string ChunkedByFive(this string s)
        {
            return string.Join(' ', s.ChunkBy(5).Select(x => new string(x.ToArray())));
        }
    }
}
