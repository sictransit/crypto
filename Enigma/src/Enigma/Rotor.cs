using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;
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
        private readonly RotorType rotorType;
        private readonly string wiring;
        private readonly int ringSetting;
        private readonly string letters;
        private readonly DoubleStepBehaviour doubleStepBehaviour;
        private bool canDoubleStep;
        private int position;
        private readonly int length;

        private readonly Dictionary<char, int> characterSet = new();

        public Rotor(RotorType rotorType, string wiring, IReadOnlyCollection<char> notches, int ringSetting = 1, string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", DoubleStepBehaviour doubleStepBehaviour = DoubleStepBehaviour.Standard)
        {
            if (notches == null || !notches.Any()) throw new ArgumentOutOfRangeException(nameof(notches));
            if (letters == null) throw new ArgumentNullException(nameof(letters));
            if (wiring == null || wiring.Length != letters.Length) throw new ArgumentOutOfRangeException(nameof(wiring));

            length = wiring.Length;

            this.notches = new HashSet<char>(notches);
            this.rotorType = rotorType;
            this.wiring = wiring;
            this.ringSetting = ringSetting - 1;
            this.letters = letters;
            this.doubleStepBehaviour = doubleStepBehaviour;

            for (var i = 0; i < wiring.Length; i++)
            {
                forwardWiring.Add(wiring[i], letters[i]);
                reverseWiring.Add(letters[i], wiring[i]);
                characterSet.Add(letters[i], i);
            }
        }

        public Rotor(RotorType type, string wiring, char notch, int ringSetting = 1)
            : this(type, wiring, new[] { notch }, ringSetting) { }

        public override EncoderType EncoderType => EncoderType.Rotor;

        public char Position => letters[position];

        public override void Attach(EnigmaDevice e, Direction direction)
        {
            base.Attach(e, direction);

            canDoubleStep = ForwardDevice?.EncoderType == EncoderType.Rotor && ReverseDevice?.EncoderType == EncoderType.Rotor;
        }

        public void SetPosition(char p)
        {
            position = letters.IndexOf(p);
        }

        public override void Tick(bool turn = false)
        {
            var notched = notches.Contains(Position);            

            if (turn || canDoubleStep && notched && (doubleStepBehaviour == DoubleStepBehaviour.Standard))
            {
                position = (position + 1) % length;
            }

            base.Tick(notched);
        }

        public override void Transpose(char c, Direction direction)
        {
            var cIn = letters[(characterSet[c] + position - ringSetting + length) % length];

            var transposed = direction == Direction.Forward ? reverseWiring[cIn] : forwardWiring[cIn];

            var cOut = letters[(characterSet[transposed] - position + ringSetting + length) % length];

            base.Transpose(cOut, direction);
        }

        public override string ToString()
        {
            var w = rotorType == RotorType.Custom ? $"{letters} → {wiring}" : rotorType.ToString();
            return $"{base.ToString()} {w} rs={letters[ringSetting]} n={string.Join(',', notches)} pos={Position}";
        }
    }
}