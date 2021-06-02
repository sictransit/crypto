using net.sictransit.crypto.enigma.Enums;
using System;
using System.Collections.Generic;

namespace net.sictransit.crypto.enigma
{
    public class PlugBoard : EncoderBase
    {
        private readonly Dictionary<char, char> wires = new();

        private readonly string wiring;

        public PlugBoard(string wiring = null)
        {
            this.wiring = wiring ?? string.Empty;

            foreach (var wire in this.wiring.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                wires.Add(wire[0], wire[1]);
                wires.Add(wire[1], wire[0]);
            }
        }

        private char Translate(char c)
        {
            return wires.TryGetValue(c, out var t) ? t : c;
        }

        public override EncoderType EncoderType => EncoderType.PlugBoard;

        public override void Transpose(char c, Direction direction)
        {
            base.Transpose(Translate(c), direction);
        }

        public override string ToString()
        {
            return $"{base.ToString()} wiring={wiring}";
        }
    }
}