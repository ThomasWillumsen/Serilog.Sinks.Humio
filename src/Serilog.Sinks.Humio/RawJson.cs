using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Serilog.Sinks.Humio
{
    [JsonConverter(typeof(JsonConverter))]
    internal sealed class RawJson
    {
        public string Value { get; }

        public RawJson(string json)
        {
            Value = json;
        }

        private sealed class JsonConverter : JsonConverter<RawJson>
        {
            public override RawJson Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                => throw new NotSupportedException("Converter does not support deserializing.");

            public override void Write(Utf8JsonWriter writer, RawJson value, JsonSerializerOptions options)
                => writer.WriteRawValue(value.Value, skipInputValidation: true);
        }
    }
}
