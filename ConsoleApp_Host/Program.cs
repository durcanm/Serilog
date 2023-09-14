using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ConsoleApp_Host
{
    internal class Program
    {
        public static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            // option #1
            //var options = host.Services.GetRequiredService<Options>();

            // option #2
            var options = new Options();
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            configuration.GetSection("Options").Bind(options);

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Warning Log");

            var sampleService = host.Services.GetRequiredService<SampleService>();
            sampleService.SampleMethod();

            var mathService = host.Services.GetRequiredService<Lib.MathService>();
            var result = mathService.Sum(7, 9);

            return host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return
                Host.CreateDefaultBuilder(args)

                    // Microsoft.Extensions.Configuration
                    .ConfigureAppConfiguration((hostingContext, builder) =>
                    {
                        IHostEnvironment
                            env = hostingContext.HostingEnvironment; // launchSettings.json -> DOTNET_ENVIRONMENT

                        builder
                            .SetBasePath(AppContext.BaseDirectory)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false,
                                reloadOnChange: true)
                            .AddEnvironmentVariables( /* prefix: "DOTNET_" */);

                        if (env.IsDevelopment()) { }
                    })

                    .ConfigureServices((hostingContext, services) =>
                    {
                        var options = new Options();
                        hostingContext.Configuration.GetSection("Options").Bind(options);
                        services.AddSingleton<Options>(options);

                        services.AddTransient<Lib.MathService>();
                        services.AddTransient<SampleService>();
                    })

                    // Microsoft Logger
                    //.ConfigureLogging(builder =>
                    //{
                    //    builder
                    //        .ClearProviders()
                    //        .SetMinimumLevel(LogLevel.Information)
                    //        .AddFilter("Microsoft", LogLevel.Error)
                    //        .AddDebug()
                    //        .AddConsole();
                    //})

                    // Serilog.Extensions.Hosting
                    .UseSerilog((hostingContext, services, loggerConfiguration) =>
                            loggerConfiguration
                                .ReadFrom.Configuration(hostingContext.Configuration) // Serilog.Settings.Configuration
                                .Enrich.FromLogContext()
                    //.WriteTo.Console()
                    )
                ;
        }
    }
}