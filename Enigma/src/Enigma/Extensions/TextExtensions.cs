using System.Linq;

namespace net.SicTransit.Crypto.Enigma.Extensions
{
    public static class TextExtensions
    {
        public static string ToEnigmaText(this string s)
        {
            return new(s.ToUpperInvariant().Where(c => c >= 'A' && c <= 'Z').ToArray());
        }

        public static string DigitsOnly(this string s)
        {
            return new(s.Where(char.IsDigit).ToArray());
        }

        public static string GroupedBy(this string s, int length = 5)
        {
            return string.Join(' ', s.Chunk(length).Select(x => new string(x.ToArray())));
        }
    }
}
