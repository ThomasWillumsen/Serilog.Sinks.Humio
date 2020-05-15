using System;
using System.Collections.Generic;

namespace Serilog.Sinks.Humio.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Humio(new HumioSinkConfiguration
                {
                    BatchSizeLimit = 50,
                    Period = TimeSpan.FromSeconds(5),
                    Tags = new KeyValuePair<string, string>[]{
                        new KeyValuePair<string, string>("source", "ApplicationLog"),
                        new KeyValuePair<string, string>("environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                    },
                    IngestToken = "{token}"
                })
                .CreateLogger();

            var position = new { Latitude = 25, Longitude = 134 };
            var elapsedMs = 34;
            log.Information("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);

            var keepAliveForDebugging = Console.ReadLine();
        }
    }
}