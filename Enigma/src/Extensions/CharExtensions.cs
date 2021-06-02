using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.sictransit.crypto.enigma.Extensions
{
    public static class CharExtensions
    {
        public static char WrapAround(this char c)
        {
            return c switch
            {
                < 'A' => (char) (c + 26),
                > 'Z' => (char) (c - 26),
                _ => c
            };
        }
    }
}
