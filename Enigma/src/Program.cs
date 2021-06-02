using Serilog;
using System;
using System.Linq;
using System.Text;

namespace net.sictransit.crypto.enigma
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logging.EnableLogging(Serilog.Events.LogEventLevel.Information);

            var plugBoard = new PlugBoard();

            var reflector = Box.SelectReflector(ReflectorType.UKW_B);

            var rotors = new[] { RotorType.I, RotorType.II, RotorType.III }.Select(x => Box.SelectRotor(x)).ToArray();

            var machine = new Enigma(plugBoard, rotors, reflector);

            var s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var sb = new StringBuilder();

            foreach (var c in s)
            {
                machine.Type(c);

                foreach (var rotor in rotors)
                {
                    Log.Debug(rotor.ToString());
                }

                sb.Append(machine.Display);
            }

            Log.Information("fuvepumwarvqkefghgdijfmfxi".ToUpper());

            Log.Information(sb.ToString());
        }
    }
}
