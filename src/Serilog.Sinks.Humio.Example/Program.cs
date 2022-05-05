using System;
using System.Collections.Generic;

namespace Serilog.Sinks.Humio.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // the simple logger has been commented out because im using the free community edition. This needs a different URL which needs
            // to be configured using the advanced initialization. RIP
            // var simpleLogger = BuildSimpleExampleLogger();
            var advancedLogger = BuildAdvancedExampleLogger();

            var position = new { Latitude = 25, Longitude = 134 };
            var elapsedMs = 34;
            // simpleLogger.Information("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            advancedLogger.Information("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);

            var keepAliveForDebugging = Console.ReadLine();
        }

        // static ILogger BuildSimpleExampleLogger()
        // {
        //     var logger = new LoggerConfiguration()
        //         .MinimumLevel.Information()
        //         .WriteTo.HumioSink("{token}")
        //         .CreateLogger();

        //     return logger;
        // }

        static ILogger BuildAdvancedExampleLogger()
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.HumioSink(new HumioSinkConfiguration
                {
                    BatchSizeLimit = 50,
                    Period = TimeSpan.FromSeconds(5),
                    Tags = new KeyValuePair<string, string>[]{
                        new KeyValuePair<string, string>("source", "ApplicationLog"),
                        new KeyValuePair<string, string>("environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                    },
                    IngestToken = "{token}",
                    Url = "https://cloud.community.humio.com"
                })
                .CreateLogger();

            return logger;
        }
    }
}