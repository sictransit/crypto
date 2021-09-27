using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;
using System;
using System.Collections.Generic;

namespace net.SicTransit.Crypto.Enigma
{
    public class Reflector : EnigmaDevice
    {
        private readonly ReflectorType reflectorType;
        private readonly Dictionary<char, char> wires = new();

        public Reflector(ReflectorType reflectorType, string wiring, string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
            if (letters == null) throw new ArgumentNullException(nameof(letters));
            if (wiring == null || wiring.Length != letters.Length) throw new ArgumentOutOfRangeException(nameof(wiring));
            this.reflectorType = reflectorType;

            for (var i = 0; i < wiring.Length; i++)
            {
                wires.Add(letters[i], wiring[i]);
            }
        }

        public override EncoderType EncoderType => EncoderType.Reflector;

        public override void Transpose(char c, Direction direction)
        {
            base.Transpose(wires[c], Direction.Reverse);
        }

        public override string ToString()
        {
            return $"{base.ToString()} {reflectorType}";
        }
    }
}