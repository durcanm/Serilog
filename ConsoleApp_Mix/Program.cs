namespace ConsoleApp_Mix
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region MS Logger

            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    //.AddFilter("ConsoleApp.Program", LogLevel.Debug)
                    //.AddFilter("Lib.MathService", LogLevel.Information)
                    .AddConsole();
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Info Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            var logger2 = loggerFactory.CreateLogger<Lib.MathService>();

            #endregion

            #region Serilog

            var serilogLogger = new LoggerConfiguration()
                .WriteTo.Console()
                //.WriteTo.File("log.txt")
                .CreateLogger();

            serilogLogger.Information("Hello from Serilog!");

            var microsoftLogger = new SerilogLoggerFactory(serilogLogger)
                .CreateLogger<Lib.MathService>();

            #endregion

            var mathservice = new Lib.MathService(microsoftLogger);

            mathservice.Sum(3, 5);
        }
    }
}