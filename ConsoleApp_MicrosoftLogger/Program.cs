using Microsoft.Extensions.Logging;

namespace ConsoleApp_MicrosoftLogger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Microsoft Logger

            // Microsoft.Extensions.Logging is referenced in Lib project, so no need to add it again!

            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddFilter("Microsoft", LogLevel.Error)
                    .AddFilter("System", LogLevel.Error)
                    .AddFilter("ConsoleApp_MicrosoftLogger.Program", LogLevel.Information)
                    .AddFilter("Lib.MathService", LogLevel.Debug)
                    //.AddJsonConsole()
                    .AddConsole();
            });

            ILogger logger1 = loggerFactory.CreateLogger<Program>();

            logger1.LogInformation("Hello from Microsoft Logger!");

            logger1.LogTrace("Trace");
            logger1.LogDebug("Debug");
            logger1.LogInformation("Info");
            logger1.LogWarning("Warning");
            logger1.LogError(3001, "Error");
            logger1.LogCritical("Critical");
            logger1.Log(LogLevel.None, "None");

            var logger2 = loggerFactory.CreateLogger<Lib.MathService>();

            #endregion

            var mathService = new Lib.MathService(logger2);

            var result = mathService.Sum(3, 5);

            logger1.LogInformation("Sum: {sum}", result);


            Console.ReadLine();
        }
    }
}