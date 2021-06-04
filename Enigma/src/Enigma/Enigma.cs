using net.SicTransit.Crypto.Enigma.Enums;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace net.SicTransit.Crypto.Enigma
{
    public class Enigma
    {
        private char[] startPositions;

        public Enigma(PlugBoard plugBoard, Rotor[] rotors, Reflector reflector)
        {
            PlugBoard = plugBoard ?? throw new ArgumentNullException(nameof(plugBoard));
            Rotors = rotors ?? throw new ArgumentNullException(nameof(rotors));
            Reflector = reflector ?? throw new ArgumentNullException(nameof(reflector));

            Keyboard = new Keyboard();

            Keyboard.Attach(PlugBoard);

            PlugBoard.Attach(Rotors[0]);

            for (var i = 0; i < Rotors.Length - 1; i++)
            {
                Rotors[i].Attach(Rotors[i + 1]);
            }

            Rotors[^1].Attach(Reflector);

            startPositions = Rotors.Select(x => x.Position).ToArray();
        }

        public Keyboard Keyboard { get; init; }

        public PlugBoard PlugBoard { get; init; }

        public Rotor[] Rotors { get; init; }

        public Reflector Reflector { get; init; }


        public void SetStartPositions(char[] positions)
        {
            if (positions == null) throw new ArgumentNullException(nameof(positions));

            if (positions.Length != Rotors.Length)
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
                Rotors[i].SetPosition(startPositions[i]);
            }
        }

        public char Display => Keyboard.ReverseChar;

        public IEnumerable<char> Type(IEnumerable<char> chars)
        {
            foreach (var c in chars)
            {
                if (TypeCharacter(c))
                {
                    yield return Display;
                }
            }
        }

        public string Transform(string s)
        {
            return new string(Type(s).ToArray());
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
                Keyboard.Tick(true);

                Keyboard.Transpose(input, Direction.Forward);

                return true;
            }

            Log.Debug($"filtered unsupported character: {c}");

            return false;
        }

        public override string ToString()
        {
            var r = string.Join(" - ", Rotors.Select((x, i) => $"{x} p0={startPositions[i]}").Reverse());

            return $"{Reflector} - {r} - {PlugBoard}";
        }
    }
}