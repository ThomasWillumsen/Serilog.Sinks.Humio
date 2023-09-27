using System.Text.Json;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Serilog.Sinks.Humio.Tests;

public class SerializationTests(ITestOutputHelper outputHelper)
{
    [Theory]
    [AutoData]
    public void Serializing_with_NewtonsoftJson_and_SystemTextJson_should_give_same_result(Fixture fixture)
    {
        // Arrange
        var sut = fixture.Create<HumioBatch>();

        // Act
        var newtonsoftJson = JsonConvert.SerializeObject(sut);
        Dump(newtonsoftJson, "Newtonsoft.Json");

        var systemTextJson = JsonSerializer.Serialize(
            sut,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        Dump(systemTextJson, "System.Text.Json");

        // Assert
        newtonsoftJson
            .Should().NotBeNull();
        systemTextJson
            .Should().NotBeNull();

        JsonDocument.Parse(systemTextJson)
                    .Should().BeEquivalentTo(JsonDocument.Parse(newtonsoftJson),
                        o => o.Using(new JsonDocumentRule()));
    }

    private void Dump(string value, string header)
    {
        outputHelper.WriteLine("################# " + header + " #################");
        outputHelper.WriteLine(value);
        outputHelper.WriteLine("################# /" + header + " #################");
    }
}
