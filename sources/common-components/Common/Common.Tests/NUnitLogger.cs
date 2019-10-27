using System;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Common.Tests
{
    public class NUnitLogger : ILogger
    {
        private readonly string _name;

        public NUnitLogger(string name)
        {
            _name = name;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            TestContext.Progress.WriteLine($"{logLevel,10}|{eventId,30}|{_name,80}|{formatter(state, exception)}");
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
