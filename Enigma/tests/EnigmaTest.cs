using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.sictransit.crypto.enigma.Enums;
using net.sictransit.crypto.enigma.Extensions;
using System.Linq;

namespace net.sictransit.crypto.enigma.tests
{
    [TestClass]
    public class EnigmaTest
    {
        private static Enigma CreateEnigma()
        {
            return CreateEnigma(new[] { RotorType.I, RotorType.II, RotorType.III }, new[] { 1, 1, 1 }, ReflectorType.UKW_B);
        }

        private static Enigma CreateEnigma(PlugBoard plugBoard)
        {
            return CreateEnigma(new[] { RotorType.I, RotorType.II, RotorType.III }, new[] { 1, 1, 1 }, ReflectorType.UKW_B, plugBoard);
        }

        private static Enigma CreateEnigma(int[] ringSettings)
        {
            return CreateEnigma(new[] { RotorType.I, RotorType.II, RotorType.III }, ringSettings, ReflectorType.UKW_B);
        }

        private static Enigma CreateEnigma(RotorType[] rotorTypes, ReflectorType reflectorType)
        {
            return CreateEnigma(rotorTypes, new[] { 1, 1, 1 }, reflectorType);
        }

        private static Enigma CreateEnigma(RotorType[] rotorTypes, int[] ringSettings, ReflectorType reflectorType, PlugBoard plugBoard = null)
        {
            var reflector = Box.SelectReflector(reflectorType);

            var rotors = rotorTypes.Select((t, i) => Box.SelectRotor(t, ringSettings[i])).ToArray();

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

            var eightBitAscii = new string(Enumerable.Range(0, 256).Select(x => (char)x).ToArray());
            const string cipherText = "FUVEPUMWARVQKEFGHGDIJFMFXIMRENATHDMCEVOQHIUWRRGYSJAD";

            Assert.AreEqual(cipherText, new string(enigma.Type(eightBitAscii).ToArray()));

            enigma.Reset();

            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ", new string(enigma.Type(cipherText).ToArray()));
        }

        [TestMethod]
        public void TestStartPositions()
        {
            var enigma = CreateEnigma();

            enigma.SetStartPositions(new[] { 'A', 'B', 'C' });

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string cipherText = "IOLIXDTUDYJEJKTXXFNAYEZQHI";

            Assert.AreEqual(cipherText, new string(enigma.Type(clearText).ToArray()));

            enigma.Reset();

            Assert.AreEqual(clearText, new string(enigma.Type(cipherText).ToArray()));
        }

        [TestMethod]
        public void TestRingSettings()
        {
            var enigma = CreateEnigma(new[] { 1, 2, 3 });

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string cipherText = "CAHZZUIFVTNDVZGJOKPXLUUNOD";

            Assert.AreEqual(cipherText, new string(enigma.Type(clearText).ToArray()));

            enigma.Reset();

            Assert.AreEqual(clearText, new string(enigma.Type(cipherText).ToArray()));
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

            Assert.AreEqual(cipherText, new string(enigma.Type(clearText).ToArray()));

            enigma.Reset();

            Assert.AreEqual(clearText.ToEnigmaText(), new string(enigma.Type(cipherText).ToArray()));
        }

        [TestMethod]
        public void TestPlugBoard()
        {
            var plugBoard = new PlugBoard(new[] { ('A', 'Z'), ('M', 'N') });

            var enigma = CreateEnigma(plugBoard);

            const string clearText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var cipherText = "tuvep unwzr vqwvf ghgdi jfnfx d".ToEnigmaText();

            Assert.AreEqual(cipherText, new string(enigma.Type(clearText).ToArray()));

            enigma.Reset();

            Assert.AreEqual(clearText, new string(enigma.Type(cipherText).ToArray()));
        }
    }
}
