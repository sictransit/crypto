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

        public static string GroupedBy(this string s, int length = 5)
        {
            return string.Join(' ', s.ChunkBy(length).Select(x => new string(x.ToArray())));
        }
    }
}
