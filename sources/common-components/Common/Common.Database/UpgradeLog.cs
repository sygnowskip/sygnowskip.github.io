using DbUp.Engine.Output;
using Microsoft.Extensions.Logging;

namespace Common.Database
{
    public class UpgradeLog<T> : IUpgradeLog
    {
        private readonly ILogger<T> _logger;

        private UpgradeLog(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void WriteInformation(string format, params object[] args)
        {
            _logger.LogInformation(string.Format(format, args));
        }

        public void WriteError(string format, params object[] args)
        {
            _logger.LogError(string.Format(format, args));
        }

        public void WriteWarning(string format, params object[] args)
        {
            _logger.LogWarning(string.Format(format, args));
        }

        public static UpgradeLog<TType> CreateFor<TType>(ILogger<TType> logger) => new UpgradeLog<TType>(logger);
    }
}