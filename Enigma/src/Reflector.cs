using net.sictransit.crypto.enigma.Enums;
using System;
using System.Collections.Generic;

namespace net.sictransit.crypto.enigma
{
    public class Reflector : EncoderBase
    {
        private readonly Dictionary<char, char> wires = new();

        public Reflector(string name, string wiring)
        {
            if (wiring == null) throw new ArgumentNullException(nameof(wiring));
            if (wiring.Length != 26) throw new ArgumentOutOfRangeException(nameof(wiring));

            Name = name ?? throw new ArgumentNullException(nameof(name));

            for (var i = 0; i < 26; i++)
            {
                wires.Add((char)('A' + i), wiring[i]);
            }
        }

        public string Name { get; }

        public override EncoderType EncoderType => EncoderType.Reflector;

        public override void Transpose(char c, Direction direction)
        {
            ForwardChar = c;

            base.Transpose(wires[c], Direction.Reverse);
        }

        public override string ToString()
        {
            return $"{base.ToString()} {Name}";
        }
    }
}