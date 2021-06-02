using System;
using System.Collections.Generic;

namespace net.sictransit.crypto.enigma
{
    public class Rotor : EncoderBase
    {
        private readonly string name;
        private readonly Dictionary<char, char> downWiring = new();
        private readonly Dictionary<char, char> upWiring = new();
        private readonly char notch;
        private readonly char turnOver;
        private int position;

        public Rotor(string name, string wiring, char notch, char turnOver)
        {
            if (wiring == null) throw new ArgumentNullException(nameof(wiring));
            if (wiring.Length != 26) throw new ArgumentOutOfRangeException(nameof(wiring));
            this.name = name ?? throw new ArgumentNullException(nameof(name));

            for (var i = 0; i < wiring.Length; i++)
            {
                upWiring.Add((char)('A' + i), wiring[i]);
                downWiring.Add(wiring[i], (char)('A' + i));
            }

            this.notch = notch;
            this.turnOver = turnOver;
        }

        public void SetPoistion(char p)
        {
            position = p - 'A';
        }

        private char PositionChar => (char)('A' + position);

        public override void Tick(bool turn = false)
        {
            base.Tick(PositionChar == turnOver);

            var doubleStep = Upstream.EncoderType == EncoderType.Rotor && Downstream.EncoderType == EncoderType.Rotor;

            if (turn || doubleStep && PositionChar == turnOver)
            {
                position = (position + 1) % 26;
            }


        }

        public override void SetUpstreamChar(char c)
        {
            var actual = (char)(c + position);

            if (actual > 'Z')
            {
                actual = (char)(actual - 26);
            }

            var upChar = upWiring[actual];

            upChar = (char)(upChar - position);

            if (upChar < 'A')
            {
                upChar = (char)(upChar + 26);
            }

            base.SetUpstreamChar(upChar);
        }

        protected override void SetDownstreamChar(char c)
        {
            var actual = (char)(c + position);

            if (actual > 'Z')
            {
                actual = (char)(actual - 26);
            }

            var downChar = downWiring[actual];

            downChar = (char)(downChar - position);

            if (downChar < 'A')
            {
                downChar = (char)(downChar + 26);
            }

            base.SetDownstreamChar(downChar);
        }


        public override EncoderType EncoderType => EncoderType.Rotor;

        public override string ToString()
        {
            return $"{base.ToString()} {name} {PositionChar} {UpstreamChar}->{DownstreamChar}";
        }
    }
}