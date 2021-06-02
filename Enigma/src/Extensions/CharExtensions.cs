using System.Linq;

namespace net.sictransit.crypto.enigma.Extensions
{
    public static class CharExtensions
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
    }
}
