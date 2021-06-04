using net.SicTransit.Crypto.Enigma.Enums;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace net.SicTransit.Crypto.Enigma
{
    public class Enigma
    {
        private readonly Keyboard keyboard = new();
        private readonly PlugBoard plugBoard;
        private readonly Rotor[] rotors;
        private readonly Reflector reflector;

        private char[] startPositions;

        public Enigma(PlugBoard plugBoard, Rotor[] rotors, Reflector reflector)
        {
            this.plugBoard = plugBoard ?? throw new ArgumentNullException(nameof(plugBoard));
            this.rotors = rotors ?? throw new ArgumentNullException(nameof(rotors));
            this.reflector = reflector ?? throw new ArgumentNullException(nameof(reflector));

            keyboard.Attach(plugBoard);

            plugBoard.Attach(rotors[0]);

            for (var i = 0; i < rotors.Length - 1; i++)
            {
                rotors[i].Attach(rotors[i + 1]);
            }

            rotors[^1].Attach(reflector);

            startPositions = rotors.Select(x => x.Position).ToArray();
        }

        public void SetStartPositions(char[] positions)
        {
            if (positions == null) throw new ArgumentNullException(nameof(positions));

            if (positions.Length != rotors.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(positions));
            }

            startPositions = positions;

            Reset();
        }

        public void Reset()
        {
            Log.Debug("resetting starting positions");

            for (var i = 0; i < startPositions.Length; i++)
            {
                rotors[i].SetPosition(startPositions[i]);
            }
        }

        public char Display => keyboard.ReverseChar;

        public IEnumerable<char> Type(IEnumerable<char> chars)
        {
            return from c in chars where TypeCharacter(c) select Display;
        }

        public void Type(char c)
        {
            TypeCharacter(c);
        }

        private bool TypeCharacter(char c)
        {
            var input = char.ToUpper(c);

            if (input >= 'A' && input <= 'Z')
            {
                keyboard.Tick(true);

                keyboard.Transpose(input, Direction.Forward);

                return true;
            }

            Log.Debug($"filtered unsupported character: {c}");

            return false;
        }

        public override string ToString()
        {
            var r = string.Join(" - ", rotors.Select((x, i) => $"{x} p0={startPositions[i]}").Reverse());

            return $"{reflector} - {r} - {plugBoard}";
        }
    }
}