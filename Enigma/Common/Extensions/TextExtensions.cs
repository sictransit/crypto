using System.Linq;

namespace net.SicTransit.Crypto.Enigma.Extensions;

public static class TextExtensions
{
    public static double IndexOfCoincidence(this string s)
    {
        return s.GroupBy(x => x).Sum(x => x.Count() * (x.Count() - 1d) / (s.Length * (s.Length - 1d)));
    }
}
