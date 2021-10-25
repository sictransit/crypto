using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;
using Serilog;

namespace net.SicTransit.Crypto.Enigma
{
    public class Lampboard : EnigmaDevice
    {
        public override EncoderType EncoderType => EncoderType.Lampboard;

        public char Lamp { get; private set; }

        public override void Transpose(char c, Direction direction)
        {
            Lamp = c;

            if (debugging)
            {
                Log.Debug($"{c}→{Name}");
            }

            base.Transpose(c, direction);
        }
    }
}