using Microsoft.Extensions.Logging;

namespace Nickel.AI.Desktop.Logging
{
    public class InMemoryLogProvider : ILoggerProvider
    {
        public readonly InMemoryLogOptions Options;

        public InMemoryLogProvider(InMemoryLogOptions options)
        {
            Options = options;
        }

        public ILogger CreateLogger(string categoryName)
        {
            // TODO: handle categoryName?
            InMemoryLog.Instance.Options = Options;
            return InMemoryLog.Instance;
        }

        public void Dispose()
        {
        }
    }
}
