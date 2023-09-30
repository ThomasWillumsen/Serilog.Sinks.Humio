using System.Text.Json;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit.Abstractions;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Serilog.Sinks.Humio.Tests;

public class SerializationTests(ITestOutputHelper outputHelper)
{
    [Theory]
    [AutoData]
    public void Serializing_with_NewtonsoftJson_and_SystemTextJson_should_give_same_result(Fixture fixture)
    {
        // Arrange
        var sut = fixture.Build<HumioLogEvent>()
                         .With(x => x.Attributes,
                             () => new RawJson(
                                 System.Text.Json.JsonSerializer.Serialize(
                                     fixture.Create<Dictionary<string, string>>())))
                         .Create<HumioBatch>();

        // Act
        var newtonsoftJson = JsonConvert.SerializeObject(sut, new RawJsonConverter());
        outputHelper.Dump(newtonsoftJson, "Newtonsoft.Json");

        var systemTextJson = System.Text.Json.JsonSerializer.Serialize(
            sut,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        outputHelper.Dump(systemTextJson, "System.Text.Json");

        // Assert
        newtonsoftJson
            .Should().NotBeNull();
        systemTextJson
            .Should().NotBeNull();

        JsonDocument.Parse(systemTextJson)
                    .Should().BeEquivalentTo(JsonDocument.Parse(newtonsoftJson),
                        o => o.Using(new JsonDocumentRule()));
    }

    private class RawJsonConverter : JsonConverter<RawJson>
    {
        public override RawJson? ReadJson(JsonReader reader, Type objectType, RawJson? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
            => throw new NotSupportedException("Converter does not support deserializing.");

        public override void WriteJson(JsonWriter writer, RawJson? value, JsonSerializer serializer)
            => writer.WriteRawValue(value?.Value);
    }
}
