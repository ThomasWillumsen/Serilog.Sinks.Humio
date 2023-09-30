using System;
using System.Collections.Generic;

namespace Serilog.Sinks.Humio
{
    internal class HumioBatch
    {
        public dynamic Tags { get; set; }

        public IEnumerable<HumioLogEvent> Events { get; set; }
    }

    internal class HumioLogEvent
    {
        public DateTimeOffset Timestamp { get; set; }

        public RawJson Attributes { get; set; }
    }
}
