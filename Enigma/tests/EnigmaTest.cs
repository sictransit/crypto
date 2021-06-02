using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace net.sictransit.crypto.enigma.tests
{
    [TestClass]
    public class EnigmaTest
    {
        private static Enigma CreateEnigma()
        {
            return CreateEnigma(new[] {RotorType.I, RotorType.II, RotorType.III}, new[] {1, 1, 1}, ReflectorType.UKW_B);
        }

        private static Enigma CreateEnigma(int[] ringSettings)
        {
            return CreateEnigma(new[] { RotorType.I, RotorType.II, RotorType.III }, ringSettings, ReflectorType.UKW_B);
        }

        private static Enigma CreateEnigma(RotorType[] rotorTypes,  ReflectorType reflectorType)
        {
            return CreateEnigma(rotorTypes, new[] { 1, 1, 1 }, reflectorType);
        }

        private static Enigma CreateEnigma(RotorType[] rotorTypes, int[] ringSettings, ReflectorType reflectorType)
        {
            var plugBoard = new PlugBoard();

            var reflector = Box.SelectReflector(reflectorType);

            var rotors = rotorTypes.Select((t, i) => Box.SelectRotor(t, ringSettings[i])).ToArray();

            return new Enigma(plugBoard, rotors, reflector);
        }

        [TestMethod]
        public void TestKnownCipherTextB321()
        {
            var enigma = CreateEnigma();

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string cipherText = "FUVEPUMWARVQKEFGHGDIJFMFXI";

            Assert.AreEqual(cipherText, new string(enigma.Type(clearText).ToArray()));

            enigma.Reset();

            Assert.AreEqual(clearText, new string(enigma.Type(cipherText).ToArray()));
        }

        [TestMethod]
        public void TestKnownCipherTextC543()
        {
            var enigma = CreateEnigma(new[] { RotorType.III, RotorType.IV, RotorType.V }, ReflectorType.UKW_C);

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string cipherText = "SIBKMIAZDAWKVEAVCZEVOJADCC";

            Assert.AreEqual(cipherText, new string(enigma.Type(clearText).ToArray()));

            enigma.Reset();

            Assert.AreEqual(clearText, new string(enigma.Type(cipherText).ToArray()));
        }

        [TestMethod]
        public void TestFilterInvalidChars()
        {
            var enigma = CreateEnigma();

            var eightBitAscii = new string(Enumerable.Range(0, 256).Select(x => (char) x).ToArray());
            const string cipherText = "FUVEPUMWARVQKEFGHGDIJFMFXIMRENATHDMCEVOQHIUWRRGYSJAD";

            Assert.AreEqual(cipherText, new string(enigma.Type(eightBitAscii).ToArray()));

            enigma.Reset();

            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ", new string(enigma.Type(cipherText).ToArray()));
        }

        [TestMethod]
        public void TestStartPositions()
        {
            var enigma = CreateEnigma();

            enigma.SetStartPositions(new[] {'A', 'B', 'C'});

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string cipherText = "IOLIXDTUDYJEJKTXXFNAYEZQHI";

            Assert.AreEqual(cipherText, new string(enigma.Type(clearText).ToArray()));

            enigma.Reset();

            Assert.AreEqual(clearText, new string(enigma.Type(cipherText).ToArray()));
        }

        [TestMethod]
        public void TestRingSettings()
        {
            var enigma = CreateEnigma(new[] {1, 2, 3});

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string cipherText = "CAHZZUIFVTNDVZGJOKPXLUUNOD";

            Assert.AreEqual(cipherText, new string(enigma.Type(clearText).ToArray()));

            enigma.Reset();

            Assert.AreEqual(clearText, new string(enigma.Type(cipherText).ToArray()));
        }
    }
}
