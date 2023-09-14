using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Json;

namespace ConsoleApp_Serilog
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Serilog

            var serilogLogger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .MinimumLevel.Override("ConsoleApp_MicrosoftLogger.Program", LogEventLevel.Information)
                .MinimumLevel.Override("Lib.MathService", LogEventLevel.Debug)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u4}] {SourceContext} {Properties} {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(new JsonFormatter(),
                    path: "log-.ndjson",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("AppName", "Serilog Demo")
                .CreateLogger();

            serilogLogger.Information("Hello from Serilog!");

            serilogLogger.Verbose("verbose");
            serilogLogger.Debug("debug");
            serilogLogger.Information("info");
            serilogLogger.Warning("warning");
            serilogLogger.Error("error");
            serilogLogger.Fatal("fatal");

            var microsoftLogger = new SerilogLoggerFactory(serilogLogger)
                .CreateLogger<Lib.MathService>();

            #endregion

            var mathService = new Lib.MathService(microsoftLogger);

            var result = mathService.Sum(7, 9);

            // sample #1
            serilogLogger.ForContext<Program>().Information("Sum: {sum}", result);

            // sample #2
            serilogLogger.ForContext("Foo", 42).Warning("Done! {@constants}", new { pi = 3.14, i = 42 });

            // sample #3
            ILogEventEnricher[] enrichers =
            {
                new PropertyEnricher("SourceContext", nameof(Program)),
                new PropertyEnricher("OSVersion", Environment.OSVersion),
                new PropertyEnricher("UserDomainName", Environment.UserDomainName)
            };
            using (LogContext.Push(enrichers))
            {
                serilogLogger.Warning("rich log!");
            }

            Console.ReadLine();
        }
    }
}