# Serilog.Sinks.Humio
A Serilog sink that writes events in periodic batches to [Humio](https://www.humio.com/).

**Package** - [Serilog.Sinks.Humio](http://nuget.org/packages/serilog.sinks.humio)  
**Platforms** - .NET Standard 2.0


The sink takes care of formatting Serilog's structured logs into a format that Humio will interpret correctly using the structured approach in Humio's [Ingest API](https://docs.humio.com/api/ingest/#structured-data).

```csharp
// Minimum default configuration
var log = new LoggerConfiguration()
    .WriteTo.HumioSink(new HumioSinkConfiguration
    {
        IngestToken = "{token}" // acquired from Humio cloud
    })
    .CreateLogger();



// Additional configuration with tags
var log = new LoggerConfiguration()
    .WriteTo.HumioSink(new HumioSinkConfiguration
    {
        IngestToken = "{token}", // acquired from Humio cloud
        BatchSizeLimit = 50,
        Period = TimeSpan.FromSeconds(5),
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
