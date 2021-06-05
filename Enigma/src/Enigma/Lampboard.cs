using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;

namespace net.SicTransit.Crypto.Enigma
{
    public class Lampboard : EnigmaDevice
    {
        public override EncoderType EncoderType => EncoderType.Lampboard;

        public char Lit { get; private set; }

        public override void Transpose(char c, Direction direction)
        {
            Lit = c;

            base.Transpose(c, direction);
        }
    }
}