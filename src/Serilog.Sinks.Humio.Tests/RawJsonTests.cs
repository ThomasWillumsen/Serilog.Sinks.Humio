using System.Text.Json;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit.Abstractions;

namespace Serilog.Sinks.Humio.Tests;

public class RawJsonTests(ITestOutputHelper outputHelper)
{
    [Theory]
    [AutoData]
    public void Should_serialize_verbatim_to_the_constructor_value(Dictionary<string, string> inputValues)
    {
        // Arrange
        var expectedJson = JsonSerializer.Serialize(inputValues);
        outputHelper.Dump(expectedJson, "expected JSON");

        var sut = new RawJson(expectedJson);

        // Act
        var actualJson = JsonSerializer.Serialize(sut);
        outputHelper.Dump(actualJson, "actual JSON");

        // Assert
        actualJson
            .Should().Be(expectedJson);
    }
}
