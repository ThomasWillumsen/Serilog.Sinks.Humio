using System;
using Serilog.Configuration;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Humio
{
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
                    new PeriodicBatchingSinkOptions{
                        BatchSizeLimit = sinkConfiguration.BatchSizeLimit,
                        Period = sinkConfiguration.Period
                    }));
        }

        /// <summary>
        /// Initialize Humio Sink
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="ingestToken">https://docs.humio.com/docs/ingesting-data/ingest-tokens/</param>
        public static LoggerConfiguration HumioSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  string ingestToken)
        {
            return loggerConfiguration.Sink(
                new PeriodicBatchingSink(
                    new HumioSink(new HumioSinkConfiguration{
                        IngestToken = ingestToken
                    }),
                    new PeriodicBatchingSinkOptions{
                        BatchSizeLimit = 100,
                        Period = TimeSpan.FromSeconds(2)
                    }));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="ingestToken"></param>
        /// <param name="url"></param>
        /// <param name="batchSizeLimit"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static LoggerConfiguration HumioSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  string ingestToken, string url, int batchSizeLimit, TimeSpan period)
        {
            return loggerConfiguration.Sink(
                new PeriodicBatchingSink(
                    new HumioSink(new HumioSinkConfiguration
                    {
                        IngestToken = ingestToken,
                        Url = url,
                    }),
                    new PeriodicBatchingSinkOptions
                    {
                        BatchSizeLimit = batchSizeLimit,
                        Period = period
                    }));
        }
    }
}
