using net.SicTransit.Crypto.Enigma.Abstract;
using net.SicTransit.Crypto.Enigma.Enums;

namespace net.SicTransit.Crypto.Enigma
{
    public class Keyboard : EncoderBase
    {
        public override EncoderType EncoderType => EncoderType.Keyboard;
    }
}