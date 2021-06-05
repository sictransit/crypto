using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;
using net.SicTransit.Crypto.Enigma.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace net.SicTransit.Crypto.Enigma
{
    public class Rotor : EncoderBase
    {
        private readonly Dictionary<char, char> forwardWiring = new();
        private readonly Dictionary<char, char> reverseWiring = new();
        private readonly char[] notches;
        private readonly int ringSetting;
        private readonly bool hasGearBox;
        private int position;
        private readonly string name;

        public Rotor(RotorType type, string wiring, IEnumerable<char> notches, int ringSetting = 1, bool hasGearBox = false)
        {
            if (wiring == null) throw new ArgumentNullException(nameof(wiring));
            if (wiring.Length != 26) throw new ArgumentOutOfRangeException(nameof(wiring));

            this.notches = notches.ToArray();
            this.ringSetting = ringSetting;
            this.hasGearBox = hasGearBox;
            name = type.ToString();

            for (var i = 0; i < wiring.Length; i++)
            {
                var wiredTo = (char)('A' + i);
                forwardWiring.Add(wiring[i], wiredTo);
                reverseWiring.Add(wiredTo, wiring[i]);
            }
        }

        public Rotor(RotorType type, string wiring, char notch, int ringSetting = 1, bool hasGearBox = false)
            : this(type, wiring, new[] { notch }, ringSetting, hasGearBox) { }

        public override EncoderType EncoderType => EncoderType.Rotor;

        private char RingSetting => (char)('A' + ringSetting - 1);

        public char Position => (char)('A' + position);

        private bool IsNotched => notches.Any(n => n == Position);

        private bool WillDoubleStep => !hasGearBox && NextEncoder.EncoderType == EncoderType.Rotor && PreviousEncoder.EncoderType == EncoderType.Rotor;

        public void SetPosition(char p)
        {
            position = p - 'A';
        }

        public override void Tick(bool turn = false)
        {
            base.Tick(IsNotched);

            if (turn || WillDoubleStep && IsNotched)
            {
                position++;
                position %= 26;
            }
        }

        public override void Transpose(char c, Direction direction)
        {
            var cIn = ((char)(c + position - ringSetting + 1)).WrapAround();

            var transposed = direction == Direction.Forward ? reverseWiring[cIn] : forwardWiring[cIn];

            var cOut = ((char)(transposed - position + ringSetting - 1)).WrapAround();

            base.Transpose(cOut, direction);
        }

        public override string ToString()
        {
            return $"{base.ToString()} {name} rs={RingSetting}";
        }
    }
}