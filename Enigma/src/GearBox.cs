using net.sictransit.crypto.enigma.Enums;
using System;

namespace net.sictransit.crypto.enigma
{
    public static class GearBox
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
                ReflectorType.UKW_A => new Reflector(type.ToString(), "EJMZALYXVBWFCRQUONTSPIKHGD"),
                ReflectorType.UKW_B => new Reflector(type.ToString(), "YRUHQSLDPXNGOKMIEBFZCWVJAT"),
                ReflectorType.UKW_C => new Reflector(type.ToString(), "FVPJIAOYEDRZXWGCTKUQSBNMHL"),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
