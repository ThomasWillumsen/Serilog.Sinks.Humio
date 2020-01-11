using Serilog.Configuration;

namespace Serilog.Sinks.Humio
{
    public static class HumioSinkExtensions
    {
        public static LoggerConfiguration HumioSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  HumioSinkConfiguration sinkConfiguration)
        {
            return loggerConfiguration.Sink(
                new HumioSink(sinkConfiguration));
        }
    }
}
