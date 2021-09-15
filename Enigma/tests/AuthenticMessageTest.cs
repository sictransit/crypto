using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.SicTransit.Crypto.Enigma.Enums;
using net.SicTransit.Crypto.Enigma.Extensions;
using System.Diagnostics;

namespace net.SicTransit.Crypto.Enigma.Tests
{
    [TestClass]
    public class AuthenticMessageTest
    {
        [TestMethod]
        public void TestEnigmaInstructionManual1930()
        {
            var enigma = EnigmaFactory.CreateEnigma(new[] { RotorType.III, RotorType.I, RotorType.II }, new[] { 22, 13, 24 }, ReflectorType.A, new Plugboard("AM FI NV PS TU WZ"));

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

                enigma.SetStartPositions(new[] { a, b, c, });

                var clearText = enigma.Transform(cipherText);

                if (clearText == exceptedClearText)
                {
                    Trace.WriteLine($"clear: {clearText}");
                    Trace.WriteLine($"elapsed: {sw.Elapsed}");

                    break;
                }
            }

            Assert.AreEqual('L', enigma.StartPositions[0]);
            Assert.AreEqual('B', enigma.StartPositions[1]);
            Assert.AreEqual('A', enigma.StartPositions[2]);
        }

        [TestMethod]
        public void TestScharnhorst1943()
        {
            var enigma = EnigmaFactory.CreateEnigma(new[] { RotorType.VIII, RotorType.VI, RotorType.III }, new[] { 13, 8, 1 }, ReflectorType.B, new Plugboard("AN EZ HK IJ LR MQ OT PV SW UX"));

            Trace.WriteLine($"enigma: {enigma}");

            var cipherText = "YKAE NZAP MSCH ZBFO CUVM RMDP YCOF HADZ IZME FXTH FLOL PZLF GGBO TGOX GRET DWTJ IQHL MXVJ WKZU ASTR";

            Trace.WriteLine($"cipher: {cipherText}");

            enigma.SetStartPositions(new[] { 'V', 'Z', 'U' });

            var clearText = enigma.Transform(cipherText);

            Trace.WriteLine($"clear: {clearText}");

            Assert.AreEqual("steuerejtanafjordjanstandortquaaacccvierneunneunzwofahrtzwonulsmxxscharnhorsthco".ToEnigmaText(), clearText);
        }


        [TestMethod]
        public void TestOperationBarbarossa1941()
        {
            var enigma = EnigmaFactory.CreateEnigma(new[] { RotorType.V, RotorType.IV, RotorType.II }, new[] { 12, 21, 2 }, ReflectorType.B, new Plugboard("AV BS CG DL FU HZ IN KM OW RX"));

            Trace.WriteLine($"enigma: {enigma}");

            var firstPartCipher = "EDPUD NRGYS ZRCXN UYTPO MRMBO FKTBZ REZKM LXLVE FGUEY SIOZV EQMIK UBPMM YLKLT TDEIS MDICA GYKUA CTCDO MOHWX MUUIA UBSTS LRNBZ SZWNR FXWFY SSXJZ VIJHI DISHP RKLKA YUPAD TXQSP INQMA TLPIF SVKDA SCTAC DPBOP VHJK-";

            Trace.WriteLine($"cipher: {firstPartCipher}");

            enigma.SetStartPositions(new[] { 'A', 'L', 'B' });

            var firstPartClear = enigma.Transform(firstPartCipher);

            Trace.WriteLine($"clear: {firstPartClear}");

            Assert.AreEqual("AUFKLXABTEILUNGXVONXKURTINOWAXKURTINOWAXNORDWESTLXSEBEZXSEBEZXUAFFLIEGERSTRASZERIQTUNGXDUBROWKIXDUBROWKIXOPOTSCHKAXOPOTSCHKAXUMXEINSAQTDREINULLXUHRANGETRETENXANGRIFFXINFXRGTX".ToEnigmaText(), firstPartClear);

            var secondPartCipher = "SFBWD NJUSE GQOBH KRTAR EEZMW KPPRB XOHDR OEQGB BGTQV PGVKB VVGBI MHUSZ YDAJQ IROAX SSSNR EHYGG RPISE ZBOVM QIEMM ZCYSG QDGRE RVBIL EKXYQ IRGIR QNRDN VRXCY YTNJR";

            Trace.WriteLine($"cipher: {secondPartCipher}");

            enigma.SetStartPositions(new[] { 'D', 'S', 'L' });

            var secondPartClear = enigma.Transform(secondPartCipher);

            Trace.WriteLine($"clear: {secondPartClear}");

            Assert.AreEqual("DREIGEHTLANGSAMABERSIQERVORWAERTSXEINSSIEBENNULLSEQSXUHRXROEMXEINSXINFRGTXDREIXAUFFLIEGERSTRASZEMITANFANGXEINSSEQSXKMXKMXOSTWXKAMENECXK".ToEnigmaText(), secondPartClear);
        }


        [TestMethod]
        public void TestU264KapitänleutnantHartwigLooks1942()
        {
            var enigma = EnigmaFactory.CreateEnigma(new[] { RotorType.I, RotorType.IV, RotorType.II, RotorType.Beta }, new[] { 22, 1, 1, 1 }, ReflectorType.ThinB, new Plugboard("AT BL DF GJ HM NW OP QY RZ VX"));

            Trace.WriteLine($"enigma: {enigma}");

            var cipher = "NCZW VUSX PNYM INHZ XMQX SFWX WLKJ AHSH NMCO CCAK UQPM KCSM HKSE INJU SBLK IOSX CKUB HMLL XCSJ USRR DVKO HULX WCCB GVLI YXEO AHXR HKKF VDRE WEZL XOBA FGYU JQUK GRTV UKAM EURB VEKS UHHV OYHA BCJW MAKL FKLM YFVN RIZR VVRT KOFD ANJM OLBG FFLE OPRG TFLV RHOW OPBE KVWM UQFM PWPA RMFH AGKX IIBG";

            Trace.WriteLine($"cipher: {cipher.ToEnigmaText().GroupedBy(4)}");

            enigma.SetStartPositions(new[] { 'A', 'N', 'J', 'V' });

            var clear = enigma.Transform(cipher.GroupedBy(4));

            Trace.WriteLine($"clear: {clear}");

            Assert.AreEqual("vonvonjlooksjhffttteinseinsdreizwoyyqnnsneuninhaltxxbeiangriffunterwassergedruecktywabosxletztergegnerstandnulachtdreinuluhrmarquantonjotaneunachtseyhsdreiyzwozwonulgradyachtsmystossenachxeknsviermbfaelltynnnnnnooovierysichteinsnull".ToEnigmaText(), clear);
        }

        [TestMethod]
        public void TestArchivedGeocacheGC2NC68()
        {
            var enigma = EnigmaFactory.CreateEnigma(
                new[] { RotorType.II, RotorType.VII, RotorType.VIII },
                new[] { 25, 19, 1 },
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

                        var clearText = enigma.Transform(cipherText);

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
    }
}
