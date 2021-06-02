using net.sictransit.crypto.enigma.Enums;
using System;

namespace net.sictransit.crypto.enigma
{
    public static class Box
    {
        public static Rotor SelectRotor(RotorType type, int ringSetting = 1)
        {
            return type switch
            {
                RotorType.I => new Rotor(type.ToString(), "EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'Q', ringSetting),
                RotorType.II => new Rotor(type.ToString(), "AJDKSIRUXBLHWTMCQGZNPYFVOE", 'E', ringSetting),
                RotorType.III => new Rotor(type.ToString(), "BDFHJLCPRTXVZNYEIWGAKMUSQO", 'V', ringSetting),
                RotorType.IV => new Rotor(type.ToString(), "ESOVPZJAYQUIRHXLNFTGKDCMWB", 'J', ringSetting),
                RotorType.V => new Rotor(type.ToString(), "VZBRGITYUPSDNHLXAWMJQOFECK", 'Z', ringSetting),
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
