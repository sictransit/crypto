using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;
using Serilog;

namespace net.SicTransit.Crypto.Enigma
{
    public class Keyboard : EnigmaDevice
    {
        public override EncoderType EncoderType => EncoderType.Keyboard;

        public override void Transpose(char c, Direction direction)
        {
            if (debugging)
            {
                Log.Debug($"{Name}→{c}");
            }

            base.Transpose(c, direction);
        }
    }
}