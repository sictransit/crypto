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
            var plugBoard = new PlugBoard(string.Join(' ', o.PlugBoard));

            var rotors = o.Rotors.Select(Enum.Parse<RotorType>)
                .Select((x, i) => GearBoxFactory.SelectRotor(x, o.RingSettings.ToArray()[i])).ToArray();
            var reflector = GearBoxFactory.SelectReflector(Enum.Parse<ReflectorType>(o.Reflector));

            var enigma = new Enigma(plugBoard, rotors, reflector);
            enigma.SetStartPositions(o.StartPositions.ToArray());

            Log.Information(enigma.ToString());

            var input = (o.Input ?? string.Empty).ToEnigmaText();

            if (!string.IsNullOrEmpty(input))
            {
                Log.Information($"input: {input.ChunkedByFive()}");

                var output = enigma.Transform(o.Input);

                Log.Information($"output: {output.ChunkedByFive()}");
            }
            else
            {
                Log.Warning("no input");
            }
        }
    }
}
