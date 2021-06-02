using net.SicTransit.Crypto.Enigma.Enums;
using net.SicTransit.Crypto.Enigma.Extensions;
using Serilog;
using System.Linq;

namespace net.SicTransit.Crypto.Enigma
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logging.EnableLogging(Serilog.Events.LogEventLevel.Debug);

            var plugBoard = new PlugBoard("EN IG MA");
            var ringSettings = new[] { 1, 7, 23 };
            var rotors = new[] { RotorType.III, RotorType.V, RotorType.IV }.Select((x, i) => GearBox.SelectRotor(x, ringSettings[i])).ToArray();

            var enigma = new Enigma(plugBoard, rotors, GearBox.SelectReflector(ReflectorType.UKW_C));
            enigma.SetStartPositions(new[] { 'A', 'L', 'N' });

            Log.Information($"settings: {enigma}");

            var cipherText = "xcsji kmiaa fxfhs esrhj lmyvf dvvkj hxvhn thxxm fhpkr cmym".ToEnigmaText();

            Log.Information($"cipher: {cipherText}");

            var clearText = enigma.Type(cipherText);

            Log.Information($"clear: {new string(clearText.ToArray())}");
        }
    }
}
