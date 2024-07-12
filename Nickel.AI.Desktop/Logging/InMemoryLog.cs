using Microsoft.Extensions.Logging;

namespace Nickel.AI.Desktop.Logging
{
    public class InMemoryLog : ILogger, IDisposable
    {
        public static readonly InMemoryLog Instance = new InMemoryLog();
        private List<LogItem> _logItems;

        private InMemoryLog()
        {
            _logItems = new List<LogItem>();
        }

        public InMemoryLogOptions? Options { get; set; }
        public List<LogItem> LogItems { get { return _logItems; } }

        public void Add(LogItem item)
        {
            // stack items at the "top" by default
            _logItems.Insert(0, item);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // TODO: allow for setting log level
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var detail = exception == null ? "" : exception.StackTrace;
#pragma warning disable CS8604 // Possible null reference argument. NOTE: linter has trouble with ternary
            var item = new LogItem(logLevel.ToString(), formatter(state, exception), detail);
#pragma warning restore CS8604 // Possible null reference argument.

            Add(item);
        }


        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            // NOTE: Don't need to handle this
            return null;
        }

        public void Dispose()
        {
            _logItems.Clear();
        }
    }
}
