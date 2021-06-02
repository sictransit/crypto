﻿using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace net.sictransit.crypto.enigma
{
    public class Enigma
    {
        private readonly Keyboard keyboard = new();
        private char[] startPositions;

        public Enigma(PlugBoard plugBoard, Rotor[] rotors, Reflector reflector)
        {
            Rotors = rotors;

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

            if (positions.Length != Rotors.Length)
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
                Rotors[i].SetPosition(startPositions[i]);
            }
        }

        public Rotor[] Rotors { get; }

        public char Display => keyboard.DownstreamChar;

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

                keyboard.SetUpstreamChar(input);

                return true;
            }

            Log.Debug("types unsupported character: {c}");

            return false;
        }
    }
}