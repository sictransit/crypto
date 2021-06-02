using net.SicTransit.Crypto.Enigma.Enums;
using System;

namespace net.SicTransit.Crypto.Enigma.Abstract
{
    public abstract class EncoderBase
    {
        protected EncoderBase NextEncoder { get; private set; }

        protected EncoderBase PreviousEncoder { get; private set; }

        public abstract EncoderType EncoderType { get; }

        protected char ForwardChar { get; set; }

        public char ReverseChar { get; private set; }

        public virtual void Transpose(char c, Direction direction)
        {
            switch (direction)
            {
                case Direction.Forward:
                    ForwardChar = c;
                    NextEncoder?.Transpose(c, direction);
                    break;
                case Direction.Reverse:
                    ReverseChar = c;
                    PreviousEncoder?.Transpose(c, direction);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void Attach(EncoderBase e)
        {
            NextEncoder = e;

            e.PreviousEncoder = this;
        }

        public virtual void Tick(bool turn = false)
        {
            NextEncoder?.Tick(turn);
        }

        public override string ToString()
        {
            return $"{EncoderType}";
        }
    }
}