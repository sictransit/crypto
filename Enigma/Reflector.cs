using System;
using System.Collections.Generic;

namespace net.sictransit.crypto.enigma
{
    internal class Reflector : EncoderBase
    {
        private readonly Dictionary<char, char> wires = new();

        public Reflector(string wiring)
        {
            if (wiring == null) throw new ArgumentNullException(nameof(wiring));
            if (wiring.Length != 26) throw new ArgumentOutOfRangeException(nameof(wiring));

            for (var i = 0; i < 26; i++)
            {
                wires.Add((char)('A' + i), wiring[i]);
            }
        }

        public override EncoderType EncoderType => EncoderType.Reflector;

        public override void SetUpstreamChar(char c)
        {
            base.SetUpstreamChar(c);

            var reflected = wires[c];

            SetDownstreamChar(reflected);
        }
    }
}