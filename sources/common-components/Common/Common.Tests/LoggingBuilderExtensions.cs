using Microsoft.Extensions.Logging;

namespace Common.Tests
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddNUnitConsoleOutput(this ILoggingBuilder logging)
        {
            logging.AddProvider(new NUnitLoggerProvider());
            return logging;
        }
    }
}