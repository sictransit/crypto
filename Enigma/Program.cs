using Serilog;
using System;
using System.Linq;

namespace net.sictransit.crypto.enigma
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logging.EnableLogging(Serilog.Events.LogEventLevel.Debug);

            var plugBoard = new PlugBoard();

            var reflector = Box.SelectReflector(ReflectorType.UKW_B);

            var rotors = new[] { RotorType.I, RotorType.II, RotorType.III }.Select(x => Box.SelectRotor(x)).ToArray();

            var machine = new Enigma(plugBoard, rotors, reflector);

            var s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            foreach (var c in s.ToCharArray())
            {
                machine.Type(c);

                foreach (var rotor in rotors)
                {
                    Log.Debug(rotor.ToString());
                }

                Console.Write(machine.Display);
            }            
        }
    }
}
