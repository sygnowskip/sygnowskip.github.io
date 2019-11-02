using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace NaturalIdentifiers.Tests.Tools
{
    public static class QueriesProvider
    {
        public static ILoggerFactory CreateFactory(SqlQueriesLogger logger) => LoggerFactory.Create(builder =>
            builder
                .AddProvider(new SqlQueriesProvider(logger)));
    }

    public class SqlQueriesProvider : ILoggerProvider
    {
        private readonly SqlQueriesLogger _logger;

        public SqlQueriesProvider(SqlQueriesLogger logger)
        {
            _logger = logger;
        }

        public void Dispose() { }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger;
        }
    }

    public class SqlQueriesLogger : ILogger
    {
        public SqlQueriesLogger()
        {
            _queries = new List<string>();
        }

        public IEnumerable<string> Queries => _queries;

        private readonly IList<string> _queries;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (eventId == RelationalEventId.CommandExecuting)
                _queries.Add(formatter(state, exception));
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