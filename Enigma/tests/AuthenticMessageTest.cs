using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.SicTransit.Crypto.Enigma.Enums;
using net.SicTransit.Crypto.Enigma.Extensions;
using Serilog;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace net.SicTransit.Crypto.Enigma.Tests
{
    [TestClass]
    public class AuthenticMessageTest
    {
        [TestMethod]
        public void TestEnigmaInstructionManual1930()
        {
            var enigma = EnigmaFactory.CreateEnigma([RotorType.II, RotorType.I, RotorType.III], [24, 13, 22], ReflectorType.A, new Plugboard("AM FI NV PS TU WZ"));

            Log.Information("enigma: {Enigma}", enigma);

            var cipherText = "GCDSE AHUGW TQGRK VLFGX UCALX VYMIG MMNMF DXTGN VHVRM MEVOU YFZSL RHDRR XFJWC FHUHM UNZEF RDISI KBGPM YVXUZ".ToEnigmaText();

            var exceptedClearText = "FEIND LIQEI NFANT ERIEK OLONN EBEOB AQTET XANFA NGSUE DAUSG ANGBA ERWAL DEXEN DEDRE IKMOS TWAER TSNEU STADT".ToEnigmaText();

            Log.Information("cipher: {CipherText}", cipherText);

            var sw = new Stopwatch();

            sw.Restart();

            for (int i = 0; i < 26 * 26 * 26; i++)
            {
                var a = (char)('A' + (i % 26));
                var b = (char)('A' + ((i / 26) % 26));
                var c = (char)('A' + ((i / 26 / 26) % 26));

                enigma.SetStartPositions([c, b, a]);

                var clearText = enigma.Transform(cipherText);

                if (clearText == exceptedClearText)
                {
                    Log.Information("clear: {ClearText}", clearText);
                    Log.Information("elapsed: {Elapsed}", sw.Elapsed);

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
            var enigma = EnigmaFactory.CreateEnigma([RotorType.III, RotorType.VI, RotorType.VIII], [1, 8, 13], ReflectorType.B, new Plugboard("AN EZ HK IJ LR MQ OT PV SW UX"));

            Log.Information("enigma: {Enigma}", enigma);

            var cipherText = "YKAE NZAP MSCH ZBFO CUVM RMDP YCOF HADZ IZME FXTH FLOL PZLF GGBO TGOX GRET DWTJ IQHL MXVJ WKZU ASTR";

            Log.Information("cipher: {CipherText}", cipherText);

            enigma.SetStartPositions(['U', 'Z', 'V']);

            var clearText = enigma.Transform(cipherText.ToEnigmaText());

            Log.Information("clear: {ClearText}", clearText);

            Assert.AreEqual("steuerejtanafjordjanstandortquaaacccvierneunneunzwofahrtzwonulsmxxscharnhorsthco".ToEnigmaText(), clearText);
        }


        [TestMethod]
        public void TestOperationBarbarossa1941()
        {
            var enigma = EnigmaFactory.CreateEnigma([RotorType.II, RotorType.IV, RotorType.V], [2, 21, 12], ReflectorType.B, new Plugboard("AV BS CG DL FU HZ IN KM OW RX"));

            Log.Information("enigma: {Enigma}", enigma);

            var firstPartCipher = "EDPUD NRGYS ZRCXN UYTPO MRMBO FKTBZ REZKM LXLVE FGUEY SIOZV EQMIK UBPMM YLKLT TDEIS MDICA GYKUA CTCDO MOHWX MUUIA UBSTS LRNBZ SZWNR FXWFY SSXJZ VIJHI DISHP RKLKA YUPAD TXQSP INQMA TLPIF SVKDA SCTAC DPBOP VHJK-";

            Log.Information("cipher: {FirstPartCipher}", firstPartCipher);

            enigma.SetStartPositions(['B', 'L', 'A']);

            var firstPartClear = enigma.Transform(firstPartCipher.ToEnigmaText());

            Log.Information("clear: {FirstPartClear}", firstPartClear);

            Assert.AreEqual("AUFKLXABTEILUNGXVONXKURTINOWAXKURTINOWAXNORDWESTLXSEBEZXSEBEZXUAFFLIEGERSTRASZERIQTUNGXDUBROWKIXDUBROWKIXOPOTSCHKAXOPOTSCHKAXUMXEINSAQTDREINULLXUHRANGETRETENXANGRIFFXINFXRGTX".ToEnigmaText(), firstPartClear);

            var secondPartCipher = "SFBWD NJUSE GQOBH KRTAR EEZMW KPPRB XOHDR OEQGB BGTQV PGVKB VVGBI MHUSZ YDAJQ IROAX SSSNR EHYGG RPISE ZBOVM QIEMM ZCYSG QDGRE RVBIL EKXYQ IRGIR QNRDN VRXCY YTNJR";

            Log.Information("cipher: {SecondPartCipher}", secondPartCipher);

            enigma.SetStartPositions(['L', 'S', 'D']);

            var secondPartClear = enigma.Transform(secondPartCipher.ToEnigmaText());

            Log.Information("clear: {SecondPartClear}", secondPartClear);

            Assert.AreEqual("DREIGEHTLANGSAMABERSIQERVORWAERTSXEINSSIEBENNULLSEQSXUHRXROEMXEINSXINFRGTXDREIXAUFFLIEGERSTRASZEMITANFANGXEINSSEQSXKMXKMXOSTWXKAMENECXK".ToEnigmaText(), secondPartClear);
        }


        [TestMethod]
        public void TestU264KapitänleutnantHartwigLooks1942()
        {
            var enigma = EnigmaFactory.CreateEnigma([RotorType.Beta, RotorType.II, RotorType.IV, RotorType.I], [1, 1, 1, 22], ReflectorType.ThinB, new Plugboard("AT BL DF GJ HM NW OP QY RZ VX"));

            Log.Information("enigma: {Enigma}", enigma);

            var cipher = "NCZW VUSX PNYM INHZ XMQX SFWX WLKJ AHSH NMCO CCAK UQPM KCSM HKSE INJU SBLK IOSX CKUB HMLL XCSJ USRR DVKO HULX WCCB GVLI YXEO AHXR HKKF VDRE WEZL XOBA FGYU JQUK GRTV UKAM EURB VEKS UHHV OYHA BCJW MAKL FKLM YFVN RIZR VVRT KOFD ANJM OLBG FFLE OPRG TFLV RHOW OPBE KVWM UQFM PWPA RMFH AGKX IIBG";

            Log.Information("cipher: {Cipher}", cipher);

            enigma.SetStartPositions(['V', 'J', 'N', 'A']);

            var clear = enigma.Transform(cipher.ToEnigmaText());

            Log.Information("clear: {Clear}", clear);

            Assert.AreEqual("vonvonjlooksjhffttteinseinsdreizwoyyqnnsneuninhaltxxbeiangriffunterwassergedruecktywabosxletztergegnerstandnulachtdreinuluhrmarquantonjotaneunachtseyhsdreiyzwozwonulgradyachtsmystossenachxeknsviermbfaelltynnnnnnooovierysichteinsnull".ToEnigmaText(), clear);
        }

        [TestMethod]
        public void TestArchivedGeocacheGC2NC68()
        {
            var enigma = EnigmaFactory.CreateEnigma(
                [RotorType.VIII, RotorType.VII, RotorType.II],
                [1, 19, 25],
                ReflectorType.B,
                new Plugboard("AS BK DU EZ FO HN IX LV QY RW"));

            Log.Information("enigma: {Enigma}", enigma);

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
                        enigma.SetStartPositions([(char)('A' + i), (char)('A' + j), (char)('A' + k)]);

                        var clearText = enigma.Transform(cipherText.ToEnigmaText());

                        var ic = clearText.IndexOfCoincidence();

                        if (ic > maxIC)
                        {
                            result = clearText;
                            maxIC = ic;

                            Log.Information("{Elapsed} [start: {StartPositions}]: (IC={IC:F6} {ClearText}", sw.Elapsed, new string(enigma.StartPositions), ic, clearText);
                        }
                    }
                }
            }

            Assert.Contains("NORTHFIVE", result);
        }

        [TestMethod]
        public void TestNumericalGeocacheMyst()
        {
            Logging.EnableLogging(Serilog.Events.LogEventLevel.Debug);

            var crib = new Regex(@"^59(?:50|51|52)[\d]{3}17(?:34|35|36|37|38|39|40)[\d]{3}$", RegexOptions.Compiled);

            var cipherText = Encoding.UTF8.GetString(Convert.FromBase64String("OTcwODQwNzk4NzgwMDU="));

            var reflector = new Reflector("X(A)", Encoding.UTF8.GetString(Convert.FromBase64String("ODc2NTQzMjEwOQ==")), "1234567890");
            var rotor1 = new Rotor("R(I)", Encoding.UTF8.GetString(Convert.FromBase64String("NzYyMzAxOTQ4NQ==")), ['4'], 1, "1234567890", false);
            var rotor2 = new Rotor("R(II)", Encoding.UTF8.GetString(Convert.FromBase64String("NTY0MjA3MzkxOA==")), ['0'], 1, "1234567890", false);
            var rotor3 = new Rotor("R(III)", Encoding.UTF8.GetString(Convert.FromBase64String("NDEyNzkwNTYzOA==")), ['4'], 1, "1234567890", false);

            var enigma = new Enigma(reflector, [rotor1, rotor2, rotor3], new Plugboard());

            enigma.SetStartPositions(['2', '0', '0']);            

            var clearText = enigma.Transform(cipherText);

            Assert.IsTrue(crib.IsMatch(clearText));

            Log.Information("{Enigma} → {ClearText}", enigma, clearText);            
        }
    }
}
