using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;
using net.SicTransit.Crypto.Enigma.Extensions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace net.SicTransit.Crypto.Enigma
{
    public class Rotor : EnigmaDevice
    {
        private readonly Dictionary<char, char> forwardWiring = new();
        private readonly Dictionary<char, char> reverseWiring = new();
        private readonly char[] notches;
        private readonly int ringSetting;
        private bool doubleStep;
        private int position;

        public Rotor(RotorType rotorType, string wiring, IEnumerable<char> notches, int ringSetting = 1)
        {
            if (wiring == null) throw new ArgumentNullException(nameof(wiring));
            if (wiring.Length != 26) throw new ArgumentOutOfRangeException(nameof(wiring));

            this.notches = notches.ToArray();
            this.ringSetting = ringSetting;

            for (var i = 0; i < wiring.Length; i++)
            {
                var wiredTo = (char)('A' + i);
                forwardWiring.Add(wiring[i], wiredTo);
                reverseWiring.Add(wiredTo, wiring[i]);
            }

            RotorType = rotorType;
        }

        public Rotor(RotorType type, string wiring, char notch, int ringSetting = 1)
            : this(type, wiring, new[] { notch }, ringSetting) { }

        public override EncoderType EncoderType => EncoderType.Rotor;

        private char RingSetting => (char)('A' + ringSetting - 1);

        public char Position => (char)('A' + position);

        private bool IsNotched => notches.Any(n => n == Position);

        public RotorType RotorType { get; }

        public override void Attach(EnigmaDevice e, Direction direction)
        {
            base.Attach(e, direction);

            doubleStep = ForwardDevice?.EncoderType == EncoderType.Rotor && ReverseDevice?.EncoderType == EncoderType.Rotor;
        }

        public void SetPosition(char p)
        {
            position = p - 'A';
        }

        public override void Tick(bool turn = false)
        {
            base.Tick(IsNotched);

            if (turn || doubleStep && IsNotched)
            {
                position++;
                position %= 26;

                Log.Debug($"turned @ {Position}: {this}");
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
            return $"{base.ToString()} {RotorType} rs={RingSetting}";
        }
    }
}