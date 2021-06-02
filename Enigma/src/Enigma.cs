using System.Collections.Generic;

namespace net.sictransit.crypto.enigma
{
    public class Enigma
    {
        private readonly Keyboard keyboard = new();

        public Enigma(PlugBoard plugBoard, Rotor[] rotors, Reflector reflector)
        {
            keyboard.Attach(plugBoard);

            plugBoard.Attach(rotors[0]);

            for (var i = 0; i < rotors.Length - 1; i++)
            {
                rotors[i].Attach(rotors[i + 1]);
            }

            rotors[^1].Attach(reflector);
        }

        public char Display { get; private set; }

        public IEnumerable<char> Type(IEnumerable<char> chars)
        {
            foreach (var c in chars)
            {
                Type(c);

                yield return Display;
            }
        }

        public void Type(char c)
        {
            keyboard.Tick(true);

            keyboard.SetUpstreamChar(c);

            Display = keyboard.DownstreamChar;
        }
    }
}