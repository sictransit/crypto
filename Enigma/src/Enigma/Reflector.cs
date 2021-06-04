using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;
using System;
using System.Collections.Generic;

namespace net.SicTransit.Crypto.Enigma
{
    public class Reflector : EncoderBase
    {
        private readonly Dictionary<char, char> wires = new();
        private readonly string name;

        public Reflector(ReflectorType type, string wiring)
        {
            if (wiring == null) throw new ArgumentNullException(nameof(wiring));
            if (wiring.Length != 26) throw new ArgumentOutOfRangeException(nameof(wiring));

            name = type.ToString();

            for (var i = 0; i < 26; i++)
            {
                wires.Add((char)('A' + i), wiring[i]);
            }
        }

        public override EncoderType EncoderType => EncoderType.Reflector;

        public override void Transpose(char c, Direction direction)
        {
            ForwardChar = c;

            base.Transpose(wires[c], Direction.Reverse);
        }

        public override string ToString()
        {
            return $"{base.ToString()} {name}";
        }
    }
}