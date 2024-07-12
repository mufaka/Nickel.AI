using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nickel.AI.Desktop.Logging
{
    public static class ILoggingBuilderExtensions
    {
        public static ILoggingBuilder AddLocalInMemoryLogger(this ILoggingBuilder builder, Action<InMemoryLogOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, InMemoryLogProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
