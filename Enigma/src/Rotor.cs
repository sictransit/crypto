using System;
using System.Collections.Generic;
using System.Linq;
using net.sictransit.crypto.enigma.Extensions;

namespace net.sictransit.crypto.enigma
{
    public class Rotor : EncoderBase
    {
        private readonly string name;
        private readonly Dictionary<char, char> downWiring = new();
        private readonly Dictionary<char, char> upWiring = new();
        private readonly char notch;
        private readonly char turnOver;
        private readonly int ringSetting;
        private int position;

        public Rotor(string name, string wiring, char notch, char turnOver, int ringSetting)
        {
            if (wiring == null) throw new ArgumentNullException(nameof(wiring));
            if (wiring.Length != 26) throw new ArgumentOutOfRangeException(nameof(wiring));
            this.name = name ?? throw new ArgumentNullException(nameof(name));

            var adjustedWiring = wiring.Skip(ringSetting - 1).Take(wiring.Length - ringSetting + 1).Concat(wiring.Take(ringSetting - 1)).ToArray();

            for (var i = 0; i < adjustedWiring.Length; i++)
            {
                upWiring.Add((char)('A' + i), adjustedWiring[i]);
                downWiring.Add(adjustedWiring[i], (char)('A' + i));
            }

            this.notch = notch;
            this.turnOver = turnOver;
            this.ringSetting = ringSetting;
        }

        public void SetPosition(char p)
        {
            position = p - 'A';
        }

        public char Position => (char)('A' + position);

        public override void Tick(bool turn = false)
        {
            base.Tick(Position == turnOver);

            var doubleStep = Upstream.EncoderType == EncoderType.Rotor && Downstream.EncoderType == EncoderType.Rotor;

            if (turn || doubleStep && Position == turnOver)
            {
                position = (position + 1) % 26;
            }
        }

        public override void SetUpstreamChar(char c)
        {
            var cIn = (char)(c + position);

            cIn = cIn.WrapAround();

            var cOut = (char) (upWiring[cIn] - position);

            cOut = cOut.WrapAround();
            
            base.SetUpstreamChar(cOut);
        }

        protected override void SetDownstreamChar(char c)
        {
            var cIn = (char)(c + position);

            cIn = cIn.WrapAround();

            var cOut = (char) (downWiring[cIn] - position);

            cOut = cOut.WrapAround();

            base.SetDownstreamChar(cOut);
        }


        public override EncoderType EncoderType => EncoderType.Rotor;

        public override string ToString()
        {
            return $"{base.ToString()} {name} {Position} {UpstreamChar}->{DownstreamChar}";
        }
    }
}