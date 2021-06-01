namespace net.sictransit.crypto.enigma
{
    internal abstract class EncoderBase
    {
        protected EncoderBase Upstream { get; private set; }

        protected EncoderBase Downstream { get; private set; }

        public abstract EncoderType EncoderType { get; }

        public char UpstreamChar { get; private set; }

        public char DownstreamChar { get; private set; }

        public virtual void SetUpstreamChar(char c)
        {
            UpstreamChar = c;

            Upstream?.SetUpstreamChar(c);
        }

        protected virtual void SetDownstreamChar(char c)
        {
            DownstreamChar = c;

            Downstream?.SetDownstreamChar(c);
        }

        public void Attach(EncoderBase e)
        {
            Upstream = e;

            e.Downstream = this;
        }

        public virtual void Tick(bool turn = false)
        {
            Upstream?.Tick(turn);
        }

        public override string ToString()
        {
            return $"{EncoderType}";
        }
    }
}