using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.SicTransit.Crypto.Enigma.Enums;
using net.SicTransit.Crypto.Enigma.Extensions;
using System;
using System.Linq;
using System.Text;

namespace net.SicTransit.Crypto.Enigma.Tests
{
    [TestClass]
    public class EnigmaTest
    {
        private static Enigma CreateEnigma()
        {
            return CreateEnigma(new[] { RotorType.I, RotorType.II, RotorType.III }, new[] { 1, 1, 1 }, ReflectorType.B);
        }

        private static Enigma CreateEnigma(PlugBoard plugBoard)
        {
            return CreateEnigma(new[] { RotorType.I, RotorType.II, RotorType.III }, new[] { 1, 1, 1 }, ReflectorType.B, plugBoard);
        }

        private static Enigma CreateEnigma(int[] ringSettings)
        {
            return CreateEnigma(new[] { RotorType.I, RotorType.II, RotorType.III }, ringSettings, ReflectorType.B);
        }

        private static Enigma CreateEnigma(RotorType[] rotorTypes, ReflectorType reflectorType)
        {
            return CreateEnigma(rotorTypes, new[] { 1, 1, 1 }, reflectorType);
        }

        private static Enigma CreateEnigma(RotorType[] rotorTypes, int[] ringSettings, ReflectorType reflectorType, PlugBoard plugBoard = null)
        {
            var reflector = GearBox.SelectReflector(reflectorType);

            var rotors = rotorTypes.Select((t, i) => GearBox.SelectRotor(t, ringSettings[i])).ToArray();

            return new Enigma(plugBoard ?? new PlugBoard(), rotors, reflector);
        }

        [TestMethod]
        public void TestTyping()
        {
            var enigma = CreateEnigma();

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string cipherText = "FUVEPUMWARVQKEFGHGDIJFMFXI";

            for (var i = 0; i < clearText.Length; i++)
            {
                enigma.Type(clearText[i]);

                Assert.AreEqual(cipherText[i], enigma.Display);
            }
        }


        [TestMethod]
        public void TestKnownCipherTextB321()
        {
            var enigma = CreateEnigma();

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string cipherText = "FUVEPUMWARVQKEFGHGDIJFMFXI";

            Assert.AreEqual(cipherText, enigma.Transform(clearText));

            enigma.Reset();

            Assert.AreEqual(clearText, enigma.Transform(cipherText));
        }

        [TestMethod]
        public void TestKnownCipherTextC543()
        {
            var enigma = CreateEnigma(new[] { RotorType.III, RotorType.IV, RotorType.V }, ReflectorType.C);

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string cipherText = "SIBKMIAZDAWKVEAVCZEVOJADCC";

            Assert.AreEqual(cipherText, enigma.Transform(clearText));

            enigma.Reset();

            Assert.AreEqual(clearText, enigma.Transform(cipherText));
        }

        [TestMethod]
        public void TestFilterInvalidChars()
        {
            var enigma = CreateEnigma();

            var eightBitAscii = new string(Enumerable.Range(0, 256).Select(x => (char)x).ToArray());
            const string cipherText = "FUVEPUMWARVQKEFGHGDIJFMFXIMRENATHDMCEVOQHIUWRRGYSJAD";

            Assert.AreEqual(cipherText, enigma.Transform(eightBitAscii));

            enigma.Reset();

            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ", enigma.Transform(cipherText));
        }

        [TestMethod]
        public void TestStartPositions()
        {
            var enigma = CreateEnigma();

            enigma.SetStartPositions(new[] { 'A', 'B', 'C' });

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string cipherText = "IOLIXDTUDYJEJKTXXFNAYEZQHI";

            Assert.AreEqual(cipherText, enigma.Transform(clearText));

            enigma.Reset();

            Assert.AreEqual(clearText, enigma.Transform(cipherText));
        }

        [TestMethod]
        public void TestRingSettings()
        {
            var enigma = CreateEnigma(new[] { 1, 2, 3 });

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string cipherText = "CAHZZUIFVTNDVZGJOKPXLUUNOD";

            Assert.AreEqual(cipherText, enigma.Transform(clearText));

            enigma.Reset();

            Assert.AreEqual(clearText, enigma.Transform(cipherText));
        }

        [TestMethod]
        public void TestLipsum()
        {
            var clearText = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam augue nulla, dapibus a bibendum eget, lobortis vitae libero. Duis aliquam nisl dapibus massa laoreet, et aliquam urna feugiat. Aliquam sagittis velit sed tristique aliquam. Nullam id dui venenatis, vestibulum leo ut, convallis purus. Proin eget odio eu ipsum malesuada euismod. Aenean nec nibh ultrices, pulvinar dui sed, porta ex. Etiam euismod eros quis sollicitudin dapibus. Morbi bibendum ullamcorper magna, sit amet iaculis odio suscipit sit amet. Sed nec ipsum augue. Nulla in arcu nec enim elementum feugiat. Nunc blandit sem at venenatis gravida.
Morbi sit amet congue lorem, ut tempor dui. In efficitur nibh nibh, non porttitor mi ultricies at. Duis condimentum posuere nunc, vitae ullamcorper lacus dignissim vulputate. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Sed efficitur nec orci quis egestas. Integer urna magna, facilisis a quam sed, faucibus fermentum mi. Fusce suscipit dui a orci tempor gravida. Praesent dapibus venenatis pharetra. Sed quis pulvinar felis. Donec id maximus nisi. Donec convallis arcu a placerat semper.
Donec pulvinar id tortor a ultricies. Quisque sed accumsan massa. Donec ullamcorper purus vel suscipit gravida. Cras nec mauris vel lorem finibus vehicula. Vivamus dapibus eget mi sed fringilla. Sed ex ante, sagittis id diam nec, accumsan fringilla ligula. Vestibulum sed velit lectus. In hac habitasse platea dictumst. Nunc tempor mollis mattis.
Proin ultricies justo non nisl euismod feugiat. In porttitor, ipsum eu feugiat efficitur, erat erat sollicitudin lorem, et laoreet mi nunc sit amet tellus. Duis suscipit nisl ac orci sodales, non tincidunt sem interdum. Curabitur non ligula a dolor hendrerit ullamcorper. Nullam in velit libero. Sed vitae eros ac nunc faucibus scelerisque ac id lorem. Duis gravida semper leo. Curabitur dignissim id dui ac commodo.
Morbi porta, lorem at molestie fermentum, mi arcu commodo diam, ut gravida arcu ipsum eu nulla. Sed eu dui suscipit, gravida risus at, vestibulum sem. Proin molestie feugiat turpis. Aenean porta posuere lorem non iaculis. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Praesent ac dictum arcu, sed placerat eros. Aliquam vestibulum ornare vestibulum. Suspendisse lobortis a massa a hendrerit. Nulla eleifend nisl in nunc finibus porttitor. Curabitur lobortis diam at interdum faucibus. Sed laoreet, massa ac efficitur pharetra, elit lectus luctus nisl, nec iaculis purus lacus et quam. Aliquam in sodales augue. Nam quam elit, ultrices in arcu at, tristique consequat nunc. Nulla vel quam vitae nibh convallis tempus eu sit amet enim. Integer dapibus lacinia ex et convallis. Phasellus in metus tortor.
                ";

            var cipherText = "ILFDFARUBDONVISRUKOZQMNDIYCOUHRLAWBRMPYLAZNYNGRWARYIEYBRYYNEIWRSGFPPYGZLSGGJQNLHSVSPZVBCUZTXSDUWCCDJURPGUWCWPVEOPSCYXYNTESGUZQYREZSNSZJMNCSLSYKYFPUMYJBARSKJSZWEHWPFLYPAEJQUXMBIVVZCCRWJKTLSHRBZXSBLSFZTOKYXVKZOYZHHTUMWPBVAYCIWMTJFJSTEWTRUCMGIBIJQGPJYPYZJTZECAGETDKALLBYXGWSVLSWRVQCGXTQVQOJPKXRQLRISNLZZVKZLWTOALBLZVRVEKHCYIMWUQJCJTBUBIVHQOIDLMFXJPHYLLAEJMNWVKNHXKQHKCNDMFRFXFFTHBYDFKRBNZBPIKHUCXRMAQFQSVXKUNGIZQMRQYQJTTBLQHWRWQKPFGVREFSBBXHJPAJYETLMSFYLOJCXDMWWBTEWSRECALTXHMQTBQTERTSFZQZEQRHVDZSRXEYBWKWEZKVMTGQVZKXJAOEHBQYTNSDDICLFIHLOQFWAKDKRPHHHLCFRPZMBWYHUPQPWFFHIDQHDCRGYLGHIQNBNXAZWTCSWUHJEPRQFLOOJAZPFBJUIJJAZDXWOZWQSJWFZWIPWASNQTYIEGQKSERHKUBKQQTMBMCVPOZOPZUABXQVXXXHSBLKJKBJLWMEBDLKNJXOAHOUGEHWCWBTCMNYZIZXCPWLNIIJXIOFFRQXZDZOPTSABIKIXMYLIGOMCDIIFDCRSYKXZZVNYEAMBMFEJNOQGOKPRGSPGPACINYNNICJCODWJJGDKURJEXRAOIIEQAVNCZWGZIXAGWNQUVIJWYSAQWADLBFBPBLFNQWWBPKIFLBENLEDPXLGLQDMLKUELTBWQDXFOCULZHAPLLKUKYVJUQIHOTGTMVKFFWUWKPUQGCNPVKYPMYUPQJKYIYNIVXBMYBJZNFFJAXXSUYJNFXRERBNRUPKCFPRWQGMNCWXNLLBXULOHTBGGXCUCGOKBRJXMRICBFPWPISPLCQVFRKMPJNKCOFMHVTASSGFEVLGRPHTZVWTWWPRVOMKQOSATNZQSURQWNNQHVDQQQGDOFVHVMATISQWNOUQQKBCGHQXRDXQQVFBLOJWHPHBGQGXXJFRVPQMVXRNQSOMKREDJORRMVSYOGPXBZKYBUHXAKGVRGZDRDVDQQXWIUYZETHVBQORIFCVSWJXPDDWLFXZSACVQWLPLNQGHYNIWSSSOAPACFUHGGLGJTDHOAWNAOHAJPNQZJHNKZCEMXVMHCKAAOQHJAPXRVZTRHVDUSEJIFWUZFRCAEJGTPKIXLYJZFZVBMSWATVYZJBLNPPNUZNJVUEZKZYEHVDUXGZWQSLWDBYOWTEMUHLYWNHICCHHPUXPRUPOCJRDVSIRTBQPQCGSQFWQJUNQDPWQMQKUCWEMXVYGYAEKSDFTFWFFEGMYPJFVJQEYMXWSLELZGCWONCINCORCNBNMZLYHXODHHEPEWUZTKBHTIXZTVRHUWNOXDSCRASTJXALTQXAQZPSCIHOOWFPHJCBATYWUHVBBCGGRYDFKWLMJWFRNRULEMQZEILHHRVIPTURJBTKCZBHOEPHNZICMLLPKBKBNKDKOQMQEIYCWUSHNQBNRNIPBCEGULNIHHBSGUWRJNVHUFJHNRNEPWNLYHWMFWMUREDPZTAGAXUTTUABYYFXVSWRKDVNQVKFFOEJWQALKFMZYNENMJRKFOYPNAAUJFQGDDOCYYWXAUWDQNLTUKUDCSPEYXDDODCRGEHYEVRVIZTGIDBRIMNHOXQVXAGFRNMRFIOVNMRVCQZCKOTQPBBNJUPTEWNFIDBSDCVKHNCOSPXLBRCSQOXEFKJVHSHAVIRASHXMQSAHFNAFAEEYZILKXUXTZNTACTBUTVEJGGIONYGQVTTHLXVPRIHWRDGYJSJLPEGNLNSRHJZUWVZPLBHUQKWILGUVGVQBWAGWOTKVKYKYWUTTBZKZCINUOSUMTQJWXLHQCXABCMTQPPLCADMZZNHWTZBCAQNTMFDUNAVHQVHYISLSYTWDJIUXGTSGCGDADAKWCFRWIZLJRRDGBEAPIKOTDOZHHSNGDSXJPMAFIUAUYOILDBOVQNDXJWGEHSTLFTZQIALBBYNQIPOOZSFHNALSYQCPKJSSEKVMLJOGCFLLFWKBZUOEKWJSFOFMGTFYWVSWUXQFADOXPURBDAOBKHWZBXMUKEYXUDPQTLBJDVVTJOWFDZNHLARTRDCELTOGQG";

            var enigma = CreateEnigma();

            Assert.AreEqual(cipherText, enigma.Transform(clearText));

            enigma.Reset();

            Assert.AreEqual(clearText.ToEnigmaText(), enigma.Transform(cipherText));
        }

        [TestMethod]
        public void TestPlugBoard()
        {
            var plugBoard = new PlugBoard("AZ MN");

            var enigma = CreateEnigma(plugBoard);

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var cipherText = "tuvep unwzr vqwvf ghgdi jfnfx d".ToEnigmaText();

            Assert.AreEqual(cipherText, enigma.Transform(clearText));

            enigma.Reset();

            Assert.AreEqual(clearText, enigma.Transform(cipherText));
        }

        [TestMethod]
        public void TestRotateAll()
        {
            var enigma = CreateEnigma();

            var rnd = new Random();

            var clearText = new string(Enumerable.Range(0, 26 * 25 * 26).Select(_ => (char)('A' + rnd.Next(26))).ToArray());

            var cipher = enigma.Transform(clearText);

            Assert.AreEqual(cipher, enigma.Transform(clearText));

            Assert.AreEqual(clearText, enigma.Transform(cipher));
        }

        [TestMethod]
        public void TestMultipleNotches()
        {
            var enigma = CreateEnigma(new[] { RotorType.VI, RotorType.VII, RotorType.VIII }, ReflectorType.B);

            Assert.IsTrue(enigma.Rotors.All(r => r.Position == 'A'));

            var cipher = "lrbmu wamnn pvdru sajzg ofbop awczp mledl qlnxi ngben pblqh odimv mmqxk nvieq wemln fjkzo qiylg eerbx ihiit elamy dobnd sdetz cdrwm kgayt hbqgb rihgp ueobi xuqwz rdqvp wodru wjnxg jicom dmale bwalh iptsb hftjh gigno tg";

            var sb = new StringBuilder();

            for (int i = 0; i < 7; i++)
            {
                sb.Append(new string(enigma.Type("ABCDEFGHIJKLMNOPQRSTUVWXYZ").ToArray()));
            }

            Assert.AreEqual(cipher.ToEnigmaText(), sb.ToString());

            Assert.AreEqual('A', enigma.Rotors[0].Position);
            Assert.AreEqual('P', enigma.Rotors[1].Position);
            Assert.AreEqual('B', enigma.Rotors[2].Position);
        }

        [TestMethod]
        public void TestLazyEncoding()
        {
            var s = new string('A', 1024);

            var enigma = CreateEnigma();

            Assert.AreEqual('A', enigma.Rotors.First().Position);

            var enumerator = enigma.Type(s);

            var clearText = enumerator.Take(10);

            Assert.AreEqual(10, clearText.Count());

            Assert.AreEqual((char)('A' + 10), enigma.Rotors.First().Position);

            clearText = enumerator.Skip(1).Take(10);

            Assert.AreEqual(10, clearText.Count());

            Assert.AreEqual((char)('A' + 10 + 1 + 10), enigma.Rotors.First().Position);
        }
    }
}
