using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nickel.AI.Desktop.Logging;
using Nickel.AI.Desktop.UI.Controls;
using Nickel.AI.Desktop.UI.Panels;

namespace Nickel.AI.Desktop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddLogging(configure =>
                    {
                        configure.AddSimpleConsole(options =>
                        {
                            options.SingleLine = true;
                            options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                        });
                        configure.AddProvider(new InMemoryLogProvider(new InMemoryLogOptions()));
                    });
                    services.AddSingleton<App>();
                    services.AddTransient<FileChooser>();
                    services.AddSingleton<ChatPanel>();
                    services.AddSingleton<ExamplePanel>();
                    services.AddSingleton<ExampleRaylibPanel>();
                    services.AddSingleton<TextExtractionPanel>();
                    services.AddSingleton<ChunkedDataPanel>();
                    services.AddSingleton<ExampleNodePanel>();
                    services.AddSingleton<ExamplePlotPanel>();
                    services.AddSingleton<VectorDbPanel>();
                }).Build();

            var app = host.Services.GetRequiredService<App>();
            App.Host = host;
            app.Run();
        }
    }
}
