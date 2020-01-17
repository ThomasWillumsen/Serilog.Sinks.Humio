using System;
using System.Collections.Generic;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace Serilog.Sinks.Humio
{
    public class HumioSinkConfiguration
    {
        /// <summary>
        /// Required.
        /// https://docs.humio.com/ingesting-data/ingest-tokens/
        /// </summary>
        public string IngestToken { get; set; }

        /// <summary>
        /// The amount of log events to ship in every batch
        /// </summary>
        public int BatchSizeLimit { get; set; } = 50;

        /// <summary>
        /// The frequency of shipping log batches
        /// </summary>
        public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Optionally, include tags for Humio
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Tags { get; set; } = null;

        /// <summary>
        /// https://docs.humio.com/ingesting-data/parsers/built-in-parsers/#serilog
        /// </summary>
        public ITextFormatter TextFormatter { get; set; } = new JsonFormatter(renderMessage: true);

        /// <summary>
        /// Optional set url for use with on-prem Humio
        /// </summary>
        public string Url { get; set; } = "https://cloud.humio.com";
    }
}
