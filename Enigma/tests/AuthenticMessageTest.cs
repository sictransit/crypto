using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.SicTransit.Crypto.Enigma.Enums;
using net.SicTransit.Crypto.Enigma.Extensions;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace net.SicTransit.Crypto.Enigma.Tests
{
    [TestClass]
    public class AuthenticMessageTest
    {
        [TestMethod]
        public void TestEnigmaInstructionManual1930()
        {
            var enigma = EnigmaFactory.CreateEnigma(new[] { RotorType.II, RotorType.I, RotorType.III }, new[] { 24, 13, 22 }, ReflectorType.A, new Plugboard("AM FI NV PS TU WZ"));

            Trace.WriteLine($"enigma: {enigma}");

            var cipherText = "GCDSE AHUGW TQGRK VLFGX UCALX VYMIG MMNMF DXTGN VHVRM MEVOU YFZSL RHDRR XFJWC FHUHM UNZEF RDISI KBGPM YVXUZ".ToEnigmaText();

            var exceptedClearText = "FEIND LIQEI NFANT ERIEK OLONN EBEOB AQTET XANFA NGSUE DAUSG ANGBA ERWAL DEXEN DEDRE IKMOS TWAER TSNEU STADT".ToEnigmaText();

            Trace.WriteLine($"cipher: {cipherText}");

            var sw = new Stopwatch();

            sw.Restart();

            for (int i = 0; i < 26 * 26 * 26; i++)
            {
                var a = (char)('A' + (i % 26));
                var b = (char)('A' + ((i / 26) % 26));
                var c = (char)('A' + ((i / 26 / 26) % 26));

                enigma.SetStartPositions(new[] { c, b, a });

                var clearText = enigma.Transform(cipherText);

                if (clearText == exceptedClearText)
                {
                    Trace.WriteLine($"clear: {clearText}");
                    Trace.WriteLine($"elapsed: {sw.Elapsed}");

                    break;
                }
            }

            Assert.AreEqual('L', enigma.StartPositions[2]);
            Assert.AreEqual('B', enigma.StartPositions[1]);
            Assert.AreEqual('A', enigma.StartPositions[0]);
        }

        [TestMethod]
        public void TestScharnhorst1943()
        {
            var enigma = EnigmaFactory.CreateEnigma(new[] { RotorType.III, RotorType.VI, RotorType.VIII }, new[] { 1, 8, 13 }, ReflectorType.B, new Plugboard("AN EZ HK IJ LR MQ OT PV SW UX"));

            Trace.WriteLine($"enigma: {enigma}");

            var cipherText = "YKAE NZAP MSCH ZBFO CUVM RMDP YCOF HADZ IZME FXTH FLOL PZLF GGBO TGOX GRET DWTJ IQHL MXVJ WKZU ASTR";

            Trace.WriteLine($"cipher: {cipherText}");

            enigma.SetStartPositions(new[] { 'U', 'Z', 'V' });

            var clearText = enigma.Transform(cipherText.ToEnigmaText());

            Trace.WriteLine($"clear: {clearText}");

            Assert.AreEqual("steuerejtanafjordjanstandortquaaacccvierneunneunzwofahrtzwonulsmxxscharnhorsthco".ToEnigmaText(), clearText);
        }


        [TestMethod]
        public void TestOperationBarbarossa1941()
        {
            var enigma = EnigmaFactory.CreateEnigma(new[] { RotorType.II, RotorType.IV, RotorType.V }, new[] { 2, 21, 12 }, ReflectorType.B, new Plugboard("AV BS CG DL FU HZ IN KM OW RX"));

            Trace.WriteLine($"enigma: {enigma}");

            var firstPartCipher = "EDPUD NRGYS ZRCXN UYTPO MRMBO FKTBZ REZKM LXLVE FGUEY SIOZV EQMIK UBPMM YLKLT TDEIS MDICA GYKUA CTCDO MOHWX MUUIA UBSTS LRNBZ SZWNR FXWFY SSXJZ VIJHI DISHP RKLKA YUPAD TXQSP INQMA TLPIF SVKDA SCTAC DPBOP VHJK-";

            Trace.WriteLine($"cipher: {firstPartCipher}");

            enigma.SetStartPositions(new[] { 'B', 'L', 'A' });

            var firstPartClear = enigma.Transform(firstPartCipher.ToEnigmaText());

            Trace.WriteLine($"clear: {firstPartClear}");

            Assert.AreEqual("AUFKLXABTEILUNGXVONXKURTINOWAXKURTINOWAXNORDWESTLXSEBEZXSEBEZXUAFFLIEGERSTRASZERIQTUNGXDUBROWKIXDUBROWKIXOPOTSCHKAXOPOTSCHKAXUMXEINSAQTDREINULLXUHRANGETRETENXANGRIFFXINFXRGTX".ToEnigmaText(), firstPartClear);

            var secondPartCipher = "SFBWD NJUSE GQOBH KRTAR EEZMW KPPRB XOHDR OEQGB BGTQV PGVKB VVGBI MHUSZ YDAJQ IROAX SSSNR EHYGG RPISE ZBOVM QIEMM ZCYSG QDGRE RVBIL EKXYQ IRGIR QNRDN VRXCY YTNJR";

            Trace.WriteLine($"cipher: {secondPartCipher}");

            enigma.SetStartPositions(new[] { 'L', 'S', 'D' });

            var secondPartClear = enigma.Transform(secondPartCipher.ToEnigmaText());

            Trace.WriteLine($"clear: {secondPartClear}");

            Assert.AreEqual("DREIGEHTLANGSAMABERSIQERVORWAERTSXEINSSIEBENNULLSEQSXUHRXROEMXEINSXINFRGTXDREIXAUFFLIEGERSTRASZEMITANFANGXEINSSEQSXKMXKMXOSTWXKAMENECXK".ToEnigmaText(), secondPartClear);
        }


        [TestMethod]
        public void TestU264KapitänleutnantHartwigLooks1942()
        {
            var enigma = EnigmaFactory.CreateEnigma(new[] { RotorType.Beta, RotorType.II, RotorType.IV, RotorType.I }, new[] { 1, 1, 1, 22 }, ReflectorType.ThinB, new Plugboard("AT BL DF GJ HM NW OP QY RZ VX"));

            Trace.WriteLine($"enigma: {enigma}");

            var cipher = "NCZW VUSX PNYM INHZ XMQX SFWX WLKJ AHSH NMCO CCAK UQPM KCSM HKSE INJU SBLK IOSX CKUB HMLL XCSJ USRR DVKO HULX WCCB GVLI YXEO AHXR HKKF VDRE WEZL XOBA FGYU JQUK GRTV UKAM EURB VEKS UHHV OYHA BCJW MAKL FKLM YFVN RIZR VVRT KOFD ANJM OLBG FFLE OPRG TFLV RHOW OPBE KVWM UQFM PWPA RMFH AGKX IIBG";

            Trace.WriteLine($"cipher: {cipher}");

            enigma.SetStartPositions(new[] { 'V', 'J', 'N', 'A' });

            var clear = enigma.Transform(cipher.ToEnigmaText());

            Trace.WriteLine($"clear: {clear}");

            Assert.AreEqual("vonvonjlooksjhffttteinseinsdreizwoyyqnnsneuninhaltxxbeiangriffunterwassergedruecktywabosxletztergegnerstandnulachtdreinuluhrmarquantonjotaneunachtseyhsdreiyzwozwonulgradyachtsmystossenachxeknsviermbfaelltynnnnnnooovierysichteinsnull".ToEnigmaText(), clear);
        }

        [TestMethod]
        public void TestArchivedGeocacheGC2NC68()
        {
            var enigma = EnigmaFactory.CreateEnigma(
                new[] { RotorType.VIII, RotorType.VII, RotorType.II },
                new[] { 1, 19, 25 },
                ReflectorType.B,
                new Plugboard("AS BK DU EZ FO HN IX LV QY RW"));

            Trace.WriteLine($"enigma: {enigma}");

            var cipherText = "OSVKA IYZML IIGEN HCAVF RUBSC INRPS YBEQB KPWCX CMZHO KONZM RGOCP TZNBL ALERX ZTVAR WEPUO FRVZI GYZXL WVLXE YXIKJ FDPLD RHFAB EANKJ FWWOB NKFPO RLUUU";

            var result = string.Empty;
            var sw = new Stopwatch();

            sw.Start();
            var maxIC = 0d;

            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        enigma.SetStartPositions(new[] { (char)('A' + i), (char)('A' + j), (char)('A' + k) });

                        var clearText = enigma.Transform(cipherText.ToEnigmaText());

                        var ic = clearText.IndexOfCoincidence();

                        if (ic > maxIC)
                        {
                            result = clearText;
                            maxIC = ic;

                            Trace.WriteLine($"{sw.Elapsed} [start: {new string(enigma.StartPositions)}]: (IC={ic:F6} {clearText}");
                        }
                    }
                }
            }

            Assert.IsTrue(result.Contains("NORTHFIVE"));
        }

        [TestMethod]
        public void TestNumericalRotors()
        {
            var crib = new Regex(@"^59(?:50|51|52)[\d]{3}17(?:34|35|36|37|38|39|40)[\d]{3}$", RegexOptions.Compiled);

            var cipherText = "97084079878005";

            var startPositions = "0123456789".ToCharArray();

            var solutions = new ConcurrentBag<string>();

            var reflector = new Reflector(ReflectorType.Custom, "8765432109", "1234567890");
            var rotor1 = new Rotor(RotorType.Custom, "7623019485", new[] { '4' }, 1, "1234567890", DoubleStepBehaviour.Inhibit);
            var rotor2 = new Rotor(RotorType.Custom, "5642073918", new[] { '0' }, 1, "1234567890", DoubleStepBehaviour.Inhibit);
            var rotor3 = new Rotor(RotorType.Custom, "4127905638", new[] { '4' }, 1, "1234567890", DoubleStepBehaviour.Inhibit);

            foreach (var x1 in new[] { rotor1, rotor2, rotor3 })
            {
                foreach (var x2 in new[] { rotor1, rotor2, rotor3 }.Except(new[] { x1 }))
                {
                    foreach (var x3 in new[] { rotor1, rotor2, rotor3 }.Except(new[] { x1, x2 }))
                    {
                        var enigma = new Enigma(reflector, new[] { x1, x2, x3 }, new Plugboard(), TickBehaviour.PostEncoding);

                        foreach (var s1 in startPositions)
                        {
                            foreach (var s2 in startPositions)
                            {
                                foreach (var s3 in startPositions)
                                {
                                    enigma.SetStartPositions(new[] { s1, s2, s3 });

                                    var clearText = enigma.Transform(cipherText);

                                    if (crib.IsMatch(clearText))
                                    {
                                        solutions.Add(clearText);
                                        Trace.WriteLine($"{enigma} → {clearText}");
                                    }
                                }
                            }
                        }

                    }
                }
            }

            foreach (var solution in solutions.Distinct().OrderBy(x => x))
            {
                Trace.WriteLine(solution);
            }
        }

        [TestMethod]
        public void TestNumericalRotors2()
        {
            var reflector = new Reflector(ReflectorType.Custom, "8765432109", "1234567890");
            var rotor1 = new Rotor(RotorType.Custom, "7623019485", new[] { '4' }, 1, "1234567890", DoubleStepBehaviour.Inhibit);
            var rotor2 = new Rotor(RotorType.Custom, "5642073918", new[] { '0' }, 1, "1234567890", DoubleStepBehaviour.Inhibit);
            var rotor3 = new Rotor(RotorType.Custom, "4127905638", new[] { '4' }, 1, "1234567890", DoubleStepBehaviour.Inhibit);

            var enigma = new Enigma(reflector, new[] { rotor1, rotor2, rotor3 }, new Plugboard(), TickBehaviour.PostEncoding);

            var cipherText = "11111111111111";

            Trace.WriteLine($"BEFORE: {enigma}");
            Trace.WriteLine($"{cipherText} → {enigma.Transform(cipherText)}");
            Trace.WriteLine($"AFTER: {enigma}");
        }
    }
}
