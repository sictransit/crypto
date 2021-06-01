using System;

namespace net.sictransit.crypto.enigma
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var plugBoard = new PlugBoard();

            //I   = EKMFLGDQVZNTOWYHXUSPAIBRCJ
            //II  = AJDKSIRUXBLHWTMCQGZNPYFVOE
            //III = BDFHJLCPRTXVZNYEIWGAKMUSQO
            //IV  = ESOVPZJAYQUIRHXLNFTGKDCMWB
            //V   = VZBRGITYUPSDNHLXAWMJQOFECK

            var rI = new Rotor("EKMFLGDQVZNTOWYHXUSPAIBRCJ", 0);
            var rII = new Rotor("AJDKSIRUXBLHWTMCQGZNPYFVOE", 0);
            var rIII = new Rotor("BDFHJLCPRTXVZNYEIWGAKMUSQO", 0);

            //Reflector B = YRUHQSLDPXNGOKMIEBFZCWVJAT
            //Reflector C = FVPJIAOYEDRZXWGCTKUQSBNMHL

            var reflector = new Reflector("FVPJIAOYEDRZXWGCTKUQSBNMHL");

            var machine = new Enigma(plugBoard, new[] { rI, rII, rIII }, reflector);

            machine.Type('A');

            Console.Write(machine.Display);
        }
    }
}
