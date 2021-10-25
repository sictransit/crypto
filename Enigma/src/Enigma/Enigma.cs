using net.SicTransit.Crypto.Enigma.Enums;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace net.SicTransit.Crypto.Enigma
{
    public class Enigma
    {
        public Enigma(Reflector reflector, Rotor[] rotors, Plugboard plugboard)
        {
            PlugBoard = plugboard ?? throw new ArgumentNullException(nameof(plugboard));
            Rotors = rotors ?? throw new ArgumentNullException(nameof(rotors));
            Reflector = reflector ?? throw new ArgumentNullException(nameof(reflector));

            Keyboard = new Keyboard();

            Lampboard = new Lampboard();

            AttachDevices();

            StartPositions = Rotors.Select(x => x.Position).ToArray();
        }

        private void AttachDevices()
        {
            Keyboard.Attach(PlugBoard, Direction.Forward);

            PlugBoard.Attach(Rotors[^1], Direction.Forward);

            for (var i = Rotors.Length - 1; i > 0; i--)
            {
                Rotors[i].Attach(Rotors[i - 1], Direction.Forward);
            }

            Rotors[0].Attach(Reflector, Direction.Forward);

            Reflector.Attach(Rotors[0], Direction.Reverse);

            for (var i = 0; i < Rotors.Length - 1; i++)
            {
                Rotors[i].Attach(Rotors[i + 1], Direction.Reverse);
            }

            Rotors[^1].Attach(PlugBoard, Direction.Reverse);

            PlugBoard.Attach(Lampboard, Direction.Reverse);
        }

        public Lampboard Lampboard { get; init; }

        public Keyboard Keyboard { get; init; }

        public Plugboard PlugBoard { get; init; }

        public Rotor[] Rotors { get; init; }

        public Reflector Reflector { get; init; }

        public char[] StartPositions { get; private set; }

        public void SetStartPositions(char[] positions)
        {
            if (positions.Length != Rotors.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(positions));
            }

            StartPositions = positions;

            Reset();
        }

        public void Reset()
        {
            Log.Debug("resetting starting positions");

            for (var i = 0; i < StartPositions.Length; i++)
            {
                Rotors[i].SetPosition(StartPositions[i]);
            }
        }

        public char Display => Lampboard.Lamp;

        public string Transform(string s)
        {
            return new string(Type(s).ToArray());
        }

        public IEnumerable<char> Type(IEnumerable<char> chars)
        {
            foreach (var c in chars)
            {
                TypeCharacter(c);

                yield return Display;
            }
        }

        public void Type(char c)
        {
            TypeCharacter(c);
        }

        private void TypeCharacter(char c)
        {
            Keyboard.Tick(true);

            Keyboard.Transpose(char.ToUpper(c), Direction.Forward);
        }

        public override string ToString()
        {
            var r = string.Join(" - ", Rotors.Select((x, i) => $"{x} p0={StartPositions[i]}"));

            return $"[{GetType().Name}] {Reflector} ↔ {r} ↔ {PlugBoard}";
        }
    }
}