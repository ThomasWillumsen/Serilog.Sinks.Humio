using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Serilog.Sinks.Humio
{
    [DataContract]
    internal class HumioBatch
    {
        [DataMember(Name = "tags")]
        public dynamic Tags { get; set; }

        [DataMember(Name = "events")]
        public IEnumerable<HumioLogEvent> Events { get; set; }
    }

    [DataContract]
    internal class HumioLogEvent
    {
        [DataMember(Name = "timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [DataMember(Name = "attributes")]
        public RawJson Attributes { get; set; }
    }
}
