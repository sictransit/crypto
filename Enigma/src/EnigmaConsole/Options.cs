using CommandLine;
using System.Collections.Generic;

namespace net.SicTransit.Crypto.Enigma
{
    internal class Options
    {
        [Option("plugboard", Required = false, Default = new string[0], HelpText = "groups of two characters, e.g. \"AZ MN\"")]
        public IEnumerable<string> PlugBoard { get; set; }

        [Option("rotors", Required = false, Default = new[] { "I", "II", "III" }, HelpText = "available rotors: I, II, III, IV, V")]
        public IEnumerable<string> Rotors { get; set; }

        [Option("reflector", Required = false, Default = "B", HelpText = "available reflectors: A, B, C")]
        public string Reflector { get; set; }

        [Option("ringsettings", Required = false, Default = new[] { 1, 1, 1 }, HelpText = "three numbers: 1-26")]
        public IEnumerable<int> RingSettings { get; set; }

        [Option("startpositions", Required = false, Default = new[] { 'A', 'A', 'A' }, HelpText = "three characters: A-Z")]
        public IEnumerable<char> StartPositions { get; set; }

        [Option("input", Required = false, Default = "Hello World", HelpText = "something to encode")]
        public string Input { get; set; }
    }
}
