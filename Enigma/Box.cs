using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.sictransit.crypto.enigma
{
    internal static class Box
    {
        public static Rotor SelectRotor(RotorType type)
        {
            return type switch
            {
                RotorType.I => new Rotor(type.ToString(), "EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'Y', 'Q'),
                RotorType.II => new Rotor(type.ToString(), "AJDKSIRUXBLHWTMCQGZNPYFVOE", 'M', 'E'),
                RotorType.III => new Rotor(type.ToString(), "BDFHJLCPRTXVZNYEIWGAKMUSQO", 'D', 'V'),
                RotorType.IV => new Rotor(type.ToString(), "ESOVPZJAYQUIRHXLNFTGKDCMWB", 'R', 'J'),
                RotorType.V => new Rotor(type.ToString(), "VZBRGITYUPSDNHLXAWMJQOFECK", 'H', 'Z'),
                _ => throw new NotImplementedException(),
            };
        }

        public static Reflector SelectReflector(ReflectorType type)
        {
            return type switch
            {
                ReflectorType.UKW_A => new Reflector("EJMZALYXVBWFCRQUONTSPIKHGD"),
                ReflectorType.UKW_B => new Reflector("YRUHQSLDPXNGOKMIEBFZCWVJAT"),
                ReflectorType.UKW_C => new Reflector("FVPJIAOYEDRZXWGCTKUQSBNMHL"),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
