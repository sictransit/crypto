using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;
using Serilog;
using System;
using System.Collections.Generic;

namespace net.SicTransit.Crypto.Enigma
{
    public class Plugboard : EnigmaDevice
    {
        private readonly Dictionary<char, char> wires = new();

        private readonly string wiring;

        public Plugboard(string wiring = null)
        {
            this.wiring = wiring ?? string.Empty;

            foreach (var wire in this.wiring.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                wires.Add(wire[0], wire[1]);
                wires.Add(wire[1], wire[0]);
            }
        }

        public override EncoderType EncoderType => EncoderType.Plugboard;

        public override void Transpose(char c, Direction direction)
        {
            var cOut = wires.TryGetValue(c, out var t) ? t : c;

            if (debugging)
            {
                Log.Debug($"{c}→{Name}→{cOut}");
            }

            base.Transpose(cOut, direction);
        }

        public override string ToString()
        {
            return $"{base.ToString()} {(string.IsNullOrEmpty(wiring) ? "()" : wiring)}";
        }
    }
}