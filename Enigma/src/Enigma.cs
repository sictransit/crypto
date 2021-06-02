using net.sictransit.crypto.enigma.Enums;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace net.sictransit.crypto.enigma
{
    public class Enigma
    {
        private readonly Keyboard keyboard = new();
        private readonly Rotor[] rotors;

        private char[] startPositions;

        public Enigma(PlugBoard plugBoard, Rotor[] rotors, Reflector reflector)
        {
            this.rotors = rotors;

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

            Log.Debug("types unsupported character: {c}");

            return false;
        }
    }
}