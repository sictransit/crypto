using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace net.sictransit.crypto.enigma.tests
{
    [TestClass]
    public class EnigmaTest
    {
        [TestMethod]
        public void TestEncryptToKnownCipherTextB321()
        {
            var plugBoard = new PlugBoard();

            var reflector = Box.SelectReflector(ReflectorType.UKW_B);

            var rotors = new[] { RotorType.I, RotorType.II, RotorType.III }.Select(Box.SelectRotor).ToArray();

            var enigma = new Enigma(plugBoard, rotors, reflector);

            var cipher = enigma.Type("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            Assert.AreEqual("FUVEPUMWARVQKEFGHGDIJFMFXI", new string(cipher.ToArray()));
        }

        [TestMethod]
        public void TestEncryptToKnownCipherTextC543()
        {
            var plugBoard = new PlugBoard();

            var reflector = Box.SelectReflector(ReflectorType.UKW_C);

            var rotors = new[] { RotorType.III, RotorType.IV, RotorType.V }.Select(Box.SelectRotor).ToArray();

            var enigma = new Enigma(plugBoard, rotors, reflector);

            var cipher = enigma.Type("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            Assert.AreEqual("SIBKMIAZDAWKVEAVCZEVOJADCC", new string(cipher.ToArray()));
        }


        [TestMethod]
        public void TestDecryptKnownCipherText()
        {
            var plugBoard = new PlugBoard();

            var reflector = Box.SelectReflector(ReflectorType.UKW_B);

            var rotors = new[] { RotorType.I, RotorType.II, RotorType.III }.Select(Box.SelectRotor).ToArray();

            var enigma = new Enigma(plugBoard, rotors, reflector);

            var cipher = enigma.Type("FUVEPUMWARVQKEFGHGDIJFMFXI");

            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", new string(cipher.ToArray()));
        }
    }
}
