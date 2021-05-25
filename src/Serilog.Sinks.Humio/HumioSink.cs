using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Humio
{
    internal class HumioSink : PeriodicBatchingSink
    {
        private readonly IEnumerable<KeyValuePair<string, string>> _tags;
        private readonly ITextFormatter _textFormatter;
        private readonly Uri _uri;
        private readonly string _token;
        private static readonly HttpClient HttpClient = new HttpClient();

        public HumioSink(HumioSinkConfiguration humioSinkConfiguration)
            : base(humioSinkConfiguration.BatchSizeLimit, humioSinkConfiguration.Period)
        {
            if (humioSinkConfiguration == null)
                throw new ArgumentNullException("humioSinkConfiguration cannot be null");

            this._tags = humioSinkConfiguration.Tags ?? new KeyValuePair<string, string>[0];
            this._textFormatter = humioSinkConfiguration.TextFormatter ?? new JsonFormatter(renderMessage: true);
            this._uri = new Uri($"{humioSinkConfiguration.Url}/api/v1/ingest/humio-structured");
            this._token = humioSinkConfiguration.IngestToken;
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> logEvents)
        {
            var batch = PrepareBatch(logEvents);

            var requestObj = new HttpRequestMessage(HttpMethod.Post, _uri);
            requestObj.Content = new StringContent(JsonConvert.SerializeObject(batch), Encoding.UTF8, "application/json");
            requestObj.Headers.Add("Authorization", $"Bearer {_token}");

            var response = await HttpClient.SendAsync(requestObj);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Failed to ship logs to Humio: {response.StatusCode} - {response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}");
        }

        protected IEnumerable<HumioBatch> PrepareBatch(IEnumerable<LogEvent> logEvents)
        {
            var tagsObj = new ExpandoObject() as IDictionary<string, object>;
            _tags
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.Key) &&
                    !string.IsNullOrWhiteSpace(x.Value))
                .ToList()
                .ForEach(x => tagsObj[x.Key] = x.Value);

            var batch = new HumioBatch[] {
                new HumioBatch
                {
                    Tags = tagsObj,
                    Events = logEvents.Select(x => {
                        var sw = new StringWriter();
                        _textFormatter.Format(x, sw);

                        return new HumioLogEvent {
                            Timestamp = x.Timestamp,
                            Attributes = JObject.Parse(sw.ToString())
                        };
                    })
                }
            };
            return batch;
        }

        protected override void Dispose(bool disposing)
        {
            HttpClient.Dispose();
            base.Dispose(disposing);
        }
    }
}