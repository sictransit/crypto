using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;
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
        private readonly HashSet<char> notches;
        private readonly int ringSetting;
        private readonly string letters;
        private readonly bool doubleSteppingEnabled;
        private bool isMiddleRotor;
        private int position;
        private readonly int length;

        private readonly Dictionary<char, int> characterSet = new();

        public Rotor(string name, string wiring, IReadOnlyCollection<char> notches, int ringSetting = 1, string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", bool enableDoubleStepping = true)
        {
            if (notches == null || !notches.Any()) throw new ArgumentOutOfRangeException(nameof(notches));
            if (letters == null) throw new ArgumentNullException(nameof(letters));
            if (wiring == null || wiring.Length != letters.Length) throw new ArgumentOutOfRangeException(nameof(wiring));

            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.notches = new HashSet<char>(notches);

            length = wiring.Length;
            this.ringSetting = ringSetting - 1;
            this.letters = letters;
            this.doubleSteppingEnabled = enableDoubleStepping;

            for (var i = 0; i < wiring.Length; i++)
            {
                forwardWiring.Add(letters[i], wiring[i]);
                reverseWiring.Add(wiring[i], letters[i]);
                characterSet.Add(letters[i], i);
            }
        }

        public Rotor(RotorType rotorType, string wiring, IReadOnlyCollection<char> notches, int ringSetting = 1, string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", bool enableDoubleStepping = true)
            : this(rotorType.ToString(), wiring, notches, ringSetting, letters, enableDoubleStepping)
        {

        }

        public Rotor(RotorType type, string wiring, char notch, int ringSetting = 1)
            : this(type, wiring, new[] { notch }, ringSetting) { }

        public override EncoderType EncoderType => EncoderType.Rotor;

        public char Position => letters[position];

        public override string Name { get; }

        public override void Attach(EnigmaDevice e, Direction direction)
        {
            base.Attach(e, direction);

            isMiddleRotor = ForwardDevice?.EncoderType == EncoderType.Rotor && ReverseDevice?.EncoderType == EncoderType.Rotor;
        }

        public void SetPosition(char p)
        {
            position = letters.IndexOf(p);
        }

        public override void Tick(bool turn = false)
        {
            var isNotched = notches.Contains(Position);

            if (doubleSteppingEnabled) // Standard Enigma!
            {
                // A rotor between rotors will "double-step", i.e. turn when notched regardless of if the previous rotor turned or not.
                var doubleStep = isNotched && isMiddleRotor;

                if (turn || doubleStep)
                {
                    position = (position + 1) % length;
                }

                base.Tick(isNotched); // If notched, turn the next rotor in sequence.
            }
            else // Strange Geocaching Enigma implementation!
            {
                if (turn)
                {
                    position = (position + 1) % length;
                }

                // When disabling double-stepping, only turn the next rotor if this is notched AND the previous rotor turned it.
                base.Tick(isNotched && turn);
            }
        }

        public override void Transpose(char c, Direction direction)
        {
            var cIn = letters[(characterSet[c] + position - ringSetting + length) % length];

            var transposed = direction == Direction.Forward ? forwardWiring[cIn] : reverseWiring[cIn];

            var cOut = letters[(characterSet[transposed] - position + ringSetting + length) % length];

            Log.Debug($"{c}({cIn})→{Name}→({transposed}){cOut}");

            base.Transpose(cOut, direction);
        }

        public override string ToString()
        {
            return $"{base.ToString()} rs={letters[ringSetting]} n={string.Join(',', notches)} pos={Position}";
        }
    }
}