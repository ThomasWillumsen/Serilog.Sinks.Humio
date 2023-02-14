# Serilog.Sinks.Humio

A Serilog sink that writes events in periodic batches to [Humio](https://www.humio.com/).

**Package** - [Serilog.Sinks.Humio](http://nuget.org/packages/serilog.sinks.humio)  
**Platforms** - .NET Standard 2.0

The sink takes care of formatting Serilog's structured logs and sending them to Humio using the structured approach in Humio's [Ingest API](https://library.humio.com/falcon-logscale/api-ingest.html#api-ingest-structured-data).

<br/>

##### Simple configuration

```csharp
var log = new LoggerConfiguration()
    .WriteTo.HumioSink("{token}") // ingest token is acquired from Humio cloud
    .CreateLogger();
```

<br/>

##### Advanced configuration

```csharp
var log = new LoggerConfiguration()
    .WriteTo.HumioSink(new HumioSinkConfiguration
    {
        IngestToken = "{token}",
        BatchSizeLimit = 50,
        Period = TimeSpan.FromSeconds(5),
        // Can be used for an on-premises hosted solution.
        // If you are using Humio Community edition the url must be set to https://cloud.community.humio.com
        Url = "https://myOnPremHumio.com",
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

<br/>

##### Using Serilog.Settings.Configuration

```csharp
var log = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
```

_Appsettings.json_

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Humio"],
    "MinimumLevel": {
      "Default": "Verbose",
      "Override": {
        "Microsoft": "Verbose",
        "Microsoft.Hosting.Lifetime": "Verbose"
      }
    },
    "WriteTo": [
      {
        "Name": "HumioSink",
        "Args": {
          "IngestToken": "581e8b6c-f5c3-4a65-a6ee-632f1165f906",
          "Url": "https://cloud.community.humio.com"
        }
      }
    ]
  }
}
```
