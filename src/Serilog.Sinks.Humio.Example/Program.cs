using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Humio;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var simpleLogger = BuildSimpleExampleLogger();
var advancedLogger = BuildAdvancedExampleLogger();
var loggerUsingSettingsConfiguration = BuildLoggerUsingSettingsConfiguration();

var position = new { Latitude = 25, Longitude = 134 };
var elapsedMs = 34;
simpleLogger.Information("(simple) Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
advancedLogger.Information("(advanced) Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
loggerUsingSettingsConfiguration.Information("(settings configuration) Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);

var keepAliveForDebugging = Console.ReadLine();


ILogger BuildSimpleExampleLogger()
{
    var logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.HumioSink(Environment.GetEnvironmentVariable("HumioIngestToken"), "https://cloud.community.humio.com")
        .CreateLogger();

    return logger;
}

ILogger BuildAdvancedExampleLogger()
{
    var logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.HumioSink(new HumioSinkConfiguration
        {
            BatchSizeLimit = 50,
            Period = TimeSpan.FromSeconds(5),
            Tags = new KeyValuePair<string, string>[]{
                new KeyValuePair<string, string>("source", "ApplicationLog"),
                new KeyValuePair<string, string>("environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!)
            },
            IngestToken = Environment.GetEnvironmentVariable("HumioIngestToken"),
            Url = "https://cloud.community.humio.com"
        })
        .CreateLogger();

    return logger;
}

ILogger BuildLoggerUsingSettingsConfiguration()
{
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

    return logger;
}