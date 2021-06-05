using net.SicTransit.Crypto.Enigma.Enums;
using System.Linq;

namespace net.SicTransit.Crypto.Enigma.Tests
{
    public static class EnigmaFactory
    {
        public static Enigma CreateEnigma()
        {
            return CreateEnigma(new[] { RotorType.I, RotorType.II, RotorType.III }, new[] { 1, 1, 1 }, ReflectorType.B);
        }

        public static Enigma CreateEnigma(Plugboard plugboard)
        {
            return CreateEnigma(new[] { RotorType.I, RotorType.II, RotorType.III }, new[] { 1, 1, 1 }, ReflectorType.B, plugboard);
        }

        public static Enigma CreateEnigma(int[] ringSettings)
        {
            return CreateEnigma(new[] { RotorType.I, RotorType.II, RotorType.III }, ringSettings, ReflectorType.B);
        }

        public static Enigma CreateEnigma(RotorType[] rotorTypes, ReflectorType reflectorType)
        {
            return CreateEnigma(rotorTypes, new[] { 1, 1, 1 }, reflectorType);
        }

        public static Enigma CreateEnigma(RotorType[] rotorTypes, int[] ringSettings, ReflectorType reflectorType, Plugboard plugboard = null)
        {
            var reflector = GearBox.SelectReflector(reflectorType);

            var rotors = rotorTypes.Select((t, i) => GearBox.SelectRotor(t, ringSettings[i])).ToArray();

            return new Enigma(plugboard ?? new Plugboard(), rotors, reflector);
        }
    }
}
