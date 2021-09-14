using System;

namespace net.SicTransit.Crypto.Enigma
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logging.EnableLogging(Serilog.Events.LogEventLevel.Information);
        }
    }
}
