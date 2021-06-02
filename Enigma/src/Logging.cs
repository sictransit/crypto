using Serilog;
using Serilog.Events;

namespace net.sictransit.crypto.enigma
{
    public static class Logging
    {
        public static void EnableLogging(LogEventLevel level = LogEventLevel.Information)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(level)
                .CreateLogger();

        }
    }
}
