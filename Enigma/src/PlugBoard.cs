using System;
using System.Collections.Generic;
using System.Linq;

namespace net.sictransit.crypto.enigma
{
    public class PlugBoard : EncoderBase
    {
        private readonly Dictionary<char, char> wires = new();

        public PlugBoard() : this(Enumerable.Empty<(char, char)>())
        {
        }

        public PlugBoard(IEnumerable<(char, char)> wiring)
        {
            if (wiring == null) throw new ArgumentNullException(nameof(wiring));

            foreach (var (from, to) in wiring)
            {
                wires.Add(from, to);
                wires.Add(to, from);
            }
        }

        private char Translate(char c)
        {
            return wires.TryGetValue(c, out var t) ? t : c;
        }

        public override EncoderType EncoderType => EncoderType.PlugBoard;

        public override void SetUpstreamChar(char c)
        {
            var translated = Translate(c);

            base.SetUpstreamChar(translated);
        }

        protected override void SetDownstreamChar(char c)
        {
            var translated = Translate(c);

            base.SetDownstreamChar(translated);
        }
    }
}