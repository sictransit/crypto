using net.SicTransit.Crypto.Enigma.Enums;
using System;

namespace net.SicTransit.Crypto.Enigma
{
    public static class GearBoxFactory
    {
        public static Rotor SelectRotor(RotorType type, int ringSetting = 1)
        {
            return type switch
            {
                RotorType.I => new Rotor(type, "EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'Q', ringSetting),
                RotorType.II => new Rotor(type, "AJDKSIRUXBLHWTMCQGZNPYFVOE", 'E', ringSetting),
                RotorType.III => new Rotor(type, "BDFHJLCPRTXVZNYEIWGAKMUSQO", 'V', ringSetting),
                RotorType.IV => new Rotor(type, "ESOVPZJAYQUIRHXLNFTGKDCMWB", 'J', ringSetting),
                RotorType.V => new Rotor(type, "VZBRGITYUPSDNHLXAWMJQOFECK", 'Z', ringSetting),
                _ => throw new NotImplementedException(),
            };
        }

        public static Reflector SelectReflector(ReflectorType type)
        {
            return type switch
            {
                ReflectorType.UKW_A => new Reflector(type, "EJMZALYXVBWFCRQUONTSPIKHGD"),
                ReflectorType.UKW_B => new Reflector(type, "YRUHQSLDPXNGOKMIEBFZCWVJAT"),
                ReflectorType.UKW_C => new Reflector(type, "FVPJIAOYEDRZXWGCTKUQSBNMHL"),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
