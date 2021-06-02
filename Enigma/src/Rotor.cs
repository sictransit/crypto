using net.sictransit.crypto.enigma.Enums;
using net.sictransit.crypto.enigma.Extensions;
using System;
using System.Collections.Generic;

namespace net.sictransit.crypto.enigma
{
    public class Rotor : EncoderBase
    {
        private readonly Dictionary<char, char> forwardWiring = new();
        private readonly Dictionary<char, char> reverseWiring = new();
        private readonly char turnOver;
        private readonly int ringSetting;
        private int position;

        public Rotor(string name, string wiring, char turnOver, int ringSetting)
        {
            if (wiring == null) throw new ArgumentNullException(nameof(wiring));
            if (wiring.Length != 26) throw new ArgumentOutOfRangeException(nameof(wiring));

            Name = name ?? throw new ArgumentNullException(nameof(name));

            for (var i = 0; i < wiring.Length; i++)
            {
                reverseWiring.Add((char)('A' + i), wiring[i]);
                forwardWiring.Add(wiring[i], (char)('A' + i));
            }

            this.turnOver = turnOver;
            this.ringSetting = ringSetting;
        }

        public string Name { get; }

        private char RingSetting => (char)('A' + ringSetting - 1);

        public void SetPosition(char p)
        {
            position = p - 'A';
        }

        public char Position => (char)('A' + position);

        private bool IsNotched => Position == turnOver;

        public override void Tick(bool turn = false)
        {
            base.Tick(IsNotched);

            var doubleStep = NextEncoder.EncoderType == EncoderType.Rotor && PreviousEncoder.EncoderType == EncoderType.Rotor;

            if (turn || doubleStep && IsNotched)
            {
                position = (position + 1) % 26;
            }
        }

        public override void Transpose(char c, Direction direction)
        {
            var cIn = (char)(c + position - ringSetting + 1);

            cIn = cIn.WrapAround();

            var transposed = direction == Direction.Forward ? reverseWiring[cIn] : forwardWiring[cIn];

            var cOut = (char)(transposed - position + ringSetting - 1);

            cOut = cOut.WrapAround();

            base.Transpose(cOut, direction);
        }

        public override EncoderType EncoderType => EncoderType.Rotor;

        public override string ToString()
        {
            return $"{base.ToString()} {Name} rs={RingSetting}";
        }
    }
}