# Serilog.Sinks.Humio
A Serilog sink that writes events in periodic batches to [Humio](https://www.humio.com/).

**Package** - [Serilog.Sinks.Humio](http://nuget.org/packages/serilog.sinks.humio)  
**Platforms** - .NET Standard 2.0


The sink takes care of formatting Serilog's structured logs and sending them to Humio using the structured approach in Humio's [Ingest API](https://docs.humio.com/reference/api/ingest/#structured-data).

```csharp
// Minimum configuration
var log = new LoggerConfiguration()
    .WriteTo.HumioSink("{token}") // ingest token is acquired from Humio cloud
    .CreateLogger();



// Advanced configuration
var log = new LoggerConfiguration()
    .WriteTo.HumioSink(new HumioSinkConfiguration
    {
        IngestToken = "{token}",
        BatchSizeLimit = 50,
        Period = TimeSpan.FromSeconds(5),
        Url = "https://myOnPremHumio.com", // for on-prem
        Tags = new KeyValuePair<string, string>[]{
            new KeyValuePair<string, string>("host", "{my_host}"),
            new KeyValuePair<string, string>("source", "{my_application}"),
            new KeyValuePair<string, string>("tag3", "some value"),
            new KeyValuePair<string, string>("tag4", "some value"),
            // ...
        }
    })
    .CreateLogger();
```
