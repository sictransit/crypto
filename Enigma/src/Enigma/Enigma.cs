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

        public Enigma(Plugboard plugboard, Rotor[] rotors, Reflector reflector)
        {
            PlugBoard = plugboard ?? throw new ArgumentNullException(nameof(plugboard));
            Rotors = rotors ?? throw new ArgumentNullException(nameof(rotors));
            Reflector = reflector ?? throw new ArgumentNullException(nameof(reflector));

            Keyboard = new Keyboard();

            Lampboard = new Lampboard();

            AttachDevices();

            startPositions = Rotors.Select(x => x.Position).ToArray();
        }

        public void AttachDevices()
        {
            Keyboard.Attach(PlugBoard, Direction.Forward);

            PlugBoard.Attach(Rotors[0], Direction.Forward);

            for (var i = 0; i < Rotors.Length - 1; i++)
            {
                Rotors[i].Attach(Rotors[i + 1], Direction.Forward);
            }

            Rotors[^1].Attach(Reflector, Direction.Forward);

            Reflector.Attach(Rotors[^1], Direction.Reverse);

            for (var i = Rotors.Length - 1; i > 0; i--)
            {
                Rotors[i].Attach(Rotors[i - 1], Direction.Reverse);
            }

            Rotors[0].Attach(PlugBoard, Direction.Reverse);

            PlugBoard.Attach(Lampboard, Direction.Reverse);
        }

        public Lampboard Lampboard { get; init; }

        public Keyboard Keyboard { get; init; }

        public Plugboard PlugBoard { get; init; }

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

        public char Display => Lampboard.Lit;

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