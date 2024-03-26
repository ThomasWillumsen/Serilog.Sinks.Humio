using System;
using Serilog.Configuration;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Humio
{
    /// <summary>
    /// Humio Sink Extensions
    /// </summary>
    public static class HumioSinkExtensions
    {
        /// <summary>
        /// Initialize Humio Sink
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="sinkConfiguration"></param>
        public static LoggerConfiguration HumioSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  HumioSinkConfiguration sinkConfiguration)
        {
            return loggerConfiguration.Sink(
                new PeriodicBatchingSink(
                    new HumioSink(sinkConfiguration),
                    new PeriodicBatchingSinkOptions
                    {
                        BatchSizeLimit = sinkConfiguration.BatchSizeLimit,
                        Period = sinkConfiguration.Period
                    }),
                    sinkConfiguration.RestrictedToMinimumLevel);
        }

        /// <summary>
        /// Initialize Humio Sink
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="ingestToken">https://docs.humio.com/docs/ingesting-data/ingest-tokens/</param>
        /// <param name="url">Defaults to https://cloud.humio.com. If using the free community edition set to https://cloud.community.humio.com</param>
        /// <param name="batchSizeLimit">The amount of log events to ship in every batch</param>
        /// <param name="period">The frequency of shipping log batches. Whichever comes first of this or batchSizeLimit</param>
        public static LoggerConfiguration HumioSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  string ingestToken, string url = "https://cloud.humio.com", int batchSizeLimit = 100, TimeSpan? period = null)
        {
            return loggerConfiguration.Sink(
                new PeriodicBatchingSink(
                    new HumioSink(new HumioSinkConfiguration
                    {
                        IngestToken = ingestToken,
                        Url = url
                    }),
                    new PeriodicBatchingSinkOptions
                    {
                        BatchSizeLimit = batchSizeLimit,
                        Period = period ?? TimeSpan.FromSeconds(2)
                    }));
        }
    }
}
