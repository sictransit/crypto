using net.SicTransit.Crypto.Enigma.Extensions;
using Serilog;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net.SicTransit.Crypto.Enigma
{
    public static class Program
    {
        private const string ClearText = @"
So if Sunday you're free
Why don't you come with me
And we'll poison the pigeons in the park
And maybe we'll do in a squirrel or two
While we're poisoning pigeons in the park
";

        public static void Main(string[] args)
        {
            Logging.EnableLogging(Serilog.Events.LogEventLevel.Information);

            var e0 = new Enigma(
                GearBox.SelectReflector(Enums.ReflectorType.B),
                new Rotor[] { GearBox.SelectRotor(Enums.RotorType.I), GearBox.SelectRotor(Enums.RotorType.III), GearBox.SelectRotor(Enums.RotorType.II) },
                new Plugboard(""));

            e0.SetStartPositions(new[] { 'Q', 'W', 'E' });

            var cipherText = e0.Transform(new string(ClearText.ToUpperInvariant().Where(c => char.IsLetter(c)).ToArray()));

            Log.Information($"Cipher text: {cipherText}");

            var rotorConfigs = new List<Enums.RotorType[]>
            {
                new[] { Enums.RotorType.I, Enums.RotorType.II, Enums.RotorType.III },
                new[] { Enums.RotorType.I, Enums.RotorType.III, Enums.RotorType.II },
                new[] { Enums.RotorType.II, Enums.RotorType.I, Enums.RotorType.III },
                new[] { Enums.RotorType.II, Enums.RotorType.III, Enums.RotorType.I },
                new[] { Enums.RotorType.III, Enums.RotorType.I, Enums.RotorType.II },
                new[] { Enums.RotorType.III, Enums.RotorType.II, Enums.RotorType.I },
            };

            var solutions = new ConcurrentBag<(double, string, string)>();

            Parallel.ForEach(rotorConfigs, c =>
            {
                for (int rs1 = 1; rs1 < 27; rs1++)
                {
                    for (int rs2 = 1; rs2 < 27; rs2++)
                    {
                        for (int rs3 = 1; rs3 < 27; rs3++)
                        {
                            var maxIC = 0d;

                            var enigma = new Enigma(
                                GearBox.SelectReflector(Enums.ReflectorType.B),
                                new Rotor[]
                                {
                                    GearBox.SelectRotor(c[0],rs1),
                                    GearBox.SelectRotor(c[1],rs2),
                                    GearBox.SelectRotor(c[2],rs3)
                                },
                                new Plugboard());

                            for (int s1 = 0; s1 < 26; s1++)
                            {
                                for (int s2 = 0; s2 < 26; s2++)
                                {
                                    for (int s3 = 0; s3 < 26; s3++)
                                    {

                                        enigma.SetStartPositions(new[]
                                            {(char) ('A' + s1), (char) ('A' + s2), (char) ('A' + s3)});

                                        var clearText = enigma.Transform(cipherText);
                                        var ic = clearText.IndexOfCoincidence();

                                        if (ic > maxIC)
                                        {
                                            Log.Debug($"{ic:F5} {clearText.Substring(0, 20)} {enigma}");
                                            maxIC = ic;

                                            solutions.Add((ic, enigma.ToString(), clearText));
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            });

            foreach (var solution in solutions.OrderByDescending(x => x.Item1).Take(20))
            {
                Log.Information($"ic: {solution.Item1} {solution.Item2} {solution.Item3}");
            }
        }
    }
}
