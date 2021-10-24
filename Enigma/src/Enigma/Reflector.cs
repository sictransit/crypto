using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;
using Serilog;
using System;
using System.Collections.Generic;

namespace net.SicTransit.Crypto.Enigma
{
    public class Reflector : EnigmaDevice
    {
        private readonly Dictionary<char, char> wires = new();

        public Reflector(string name, string wiring, string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
            if (letters == null) throw new ArgumentNullException(nameof(letters));
            if (wiring == null || wiring.Length != letters.Length) throw new ArgumentOutOfRangeException(nameof(wiring));

            Name = name ?? throw new ArgumentNullException(nameof(name));

            for (var i = 0; i < wiring.Length; i++)
            {
                wires.Add(letters[i], wiring[i]);
            }
        }

        public Reflector(ReflectorType reflectorType, string wiring, string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            : this(reflectorType.ToString(), wiring, letters)
        {
        }

        public override EncoderType EncoderType => EncoderType.Reflector;

        public override string Name { get; }

        public override void Transpose(char c, Direction direction)
        {
            var cOut = wires[c];

            Log.Debug($"{c}→{Name}→{cOut}");

            base.Transpose(cOut, Direction.Reverse);
        }
    }
}