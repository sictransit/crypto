using System;

namespace net.sictransit.crypto.enigma
{
    public static class Box
    {
        public static Rotor SelectRotor(RotorType type, int ringSetting = 1)
        {
            return type switch
            {
                RotorType.I => new Rotor(type.ToString(), "EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'Y', 'Q', ringSetting),
                RotorType.II => new Rotor(type.ToString(), "AJDKSIRUXBLHWTMCQGZNPYFVOE", 'M', 'E', ringSetting),
                RotorType.III => new Rotor(type.ToString(), "BDFHJLCPRTXVZNYEIWGAKMUSQO", 'D', 'V', ringSetting),
                RotorType.IV => new Rotor(type.ToString(), "ESOVPZJAYQUIRHXLNFTGKDCMWB", 'R', 'J', ringSetting),
                RotorType.V => new Rotor(type.ToString(), "VZBRGITYUPSDNHLXAWMJQOFECK", 'H', 'Z', ringSetting),
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
