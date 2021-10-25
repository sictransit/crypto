using CommandLine;
using net.SicTransit.Crypto.Enigma.Enums;
using net.SicTransit.Crypto.Enigma.Extensions;
using Serilog;
using System;
using System.Linq;

namespace net.SicTransit.Crypto.Enigma
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logging.EnableLogging(Serilog.Events.LogEventLevel.Information);

            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                try
                {
                    Encrypt(o);
                }
                catch (Exception e)
                {
                    Log.Error(e, "caught exception while setting up the machine");
                }
            });
        }

        private static void Encrypt(Options o)
        {
            var plugboard = new FoBoard(string.Join(' ', o.PlugBoard));

            var rotors = o.Rotors.Select(Enum.Parse<RotorType>)
                .Select((x, i) => GearBox.SelectRotor(x, o.RingSettings.ToArray()[i])).ToArray();
            var reflector = GearBox.SelectReflector(Enum.Parse<ReflectorType>(o.Reflector));

            var enigma = new Enigma(reflector, rotors, plugboard);
            enigma.SetStartPositions(o.StartPositions.ToArray());

            Log.Information(enigma.ToString());

            var input = (o.Input ?? string.Empty).ToEnigmaText();

            if (!string.IsNullOrEmpty(input))
            {
                Log.Information($"input: {input.GroupedBy()}");

                var output = enigma.Transform(input);

                Log.Information($"output: {output.GroupedBy()}");
            }
            else
            {
                Log.Warning("no input");
            }
        }
    }
}
