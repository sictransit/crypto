using net.SicTransit.Crypto.Enigma.Enums;
using Serilog;
using System;

namespace net.SicTransit.Crypto.Enigma.Abstract;

public abstract class EnigmaDevice
{
    protected readonly bool debugging;

    public EnigmaDevice()
    {
        debugging = Log.IsEnabled(Serilog.Events.LogEventLevel.Debug);
    }

    protected EnigmaDevice ForwardDevice { get; private set; }

    protected EnigmaDevice ReverseDevice { get; private set; }

    public abstract EncoderType EncoderType { get; }

    public virtual string Name => EncoderType.ToString();

    public virtual void Transpose(char c, Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                ForwardDevice?.Transpose(c, direction);
                break;
            case Direction.Reverse:
                ReverseDevice?.Transpose(c, direction);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    public virtual void Attach(EnigmaDevice e, Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                ForwardDevice = e;
                break;
            case Direction.Reverse:
                ReverseDevice = e;
                break;
            default:
                throw new NotImplementedException(direction.ToString());
        }
    }

    public virtual void Tick(bool turn = false)
    {
        ForwardDevice?.Tick(turn);
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}