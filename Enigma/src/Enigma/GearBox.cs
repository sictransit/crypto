using net.SicTransit.Crypto.Enigma.Enums;
using System;

namespace net.SicTransit.Crypto.Enigma
{
    public static class GearBox
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
                RotorType.VI => new Rotor(type, "JPGVOUMFYQBENHZRDKASXLICTW", new[] { 'Z', 'M' }, ringSetting),
                RotorType.VII => new Rotor(type, "NZJHGRCXMYSWBOUFAIVLPEKQDT", new[] { 'Z', 'M' }, ringSetting),
                RotorType.VIII => new Rotor(type, "FKQHTLXOCBJSPDZRAMEWNIUYGV", new[] { 'Z', 'M' }, ringSetting),
                _ => throw new NotImplementedException(),
            };
        }

        public static Reflector SelectReflector(ReflectorType type)
        {
            return type switch
            {
                ReflectorType.A => new Reflector(type, "EJMZALYXVBWFCRQUONTSPIKHGD"),
                ReflectorType.B => new Reflector(type, "YRUHQSLDPXNGOKMIEBFZCWVJAT"),
                ReflectorType.C => new Reflector(type, "FVPJIAOYEDRZXWGCTKUQSBNMHL"),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
