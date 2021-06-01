using System;

namespace net.sictransit.crypto.enigma
{
    internal class Rotor : EncoderBase
    {
        private readonly string wiring;
        private readonly int turnOver;
        private int ticks;

        public Rotor(string wiring, int turnOver)
        {
            if (wiring == null) throw new ArgumentNullException(nameof(wiring));
            if (wiring.Length != 26) throw new ArgumentOutOfRangeException(nameof(wiring));
            this.wiring = wiring;
            this.turnOver = turnOver;
        }

        public override void Tick(bool turn = false)
        {
            var doubleStep = Upstream.EncoderType == EncoderType.Rotor && Downstream.EncoderType == EncoderType.Rotor;

            if (turn || doubleStep && ticks == turnOver)
            {
                ticks = (ticks + 1) % 26;
            }

            base.Tick(ticks == turnOver);
        }

        public override void SetUpstreamChar(char c)
        {
            var index = (c - 'A' + ticks) % 26;

            base.SetUpstreamChar(wiring[index]);
        }

        public override EncoderType EncoderType => EncoderType.Rotor;
    }
}