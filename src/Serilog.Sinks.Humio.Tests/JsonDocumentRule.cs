using System.Text.Json;
using FluentAssertions.Equivalency;

namespace Serilog.Sinks.Humio.Tests;

public class JsonDocumentRule : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
    {
        switch (comparands)
        {
            case ( { Subject: JsonDocument subjectValue, Expectation: JsonDocument expectationValue }):
                nestedValidator.RecursivelyAssertEquality(
                    new Comparands(subjectValue.RootElement, expectationValue.RootElement, typeof(JsonElement)),
                    context);
                return EquivalencyResult.AssertionCompleted;

            case { Subject: JsonElement subject, Expectation: JsonElement expectation } when subject.ValueKind == expectation.ValueKind:
                return HandleJsonElements(subject, expectation, context, nestedValidator);

            case { Subject: JsonProperty subject, Expectation: JsonProperty expectation }:
                nestedValidator.RecursivelyAssertEquality(
                    new Comparands(subject.Name, expectation.Name, typeof(string)),
                    context);
                nestedValidator.RecursivelyAssertEquality(
                    new Comparands(subject.Value, expectation.Value, typeof(JsonElement)),
                    context);

                return EquivalencyResult.AssertionCompleted;

            // equality assertion handled
            default:
                return EquivalencyResult.ContinueWithNext;
        }

        static EquivalencyResult HandleJsonElements(JsonElement subject, JsonElement expectation, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            switch (subject.ValueKind)
            {
                case JsonValueKind.Undefined:
                case JsonValueKind.True:
                case JsonValueKind.False:
                case JsonValueKind.Null:
                    return EquivalencyResult.AssertionCompleted;

                case JsonValueKind.Object:
                    nestedValidator.RecursivelyAssertEquality(
                        new Comparands(subject.EnumerateObject().ToDictionary(p => p.Name, p => p.Value),
                            expectation.EnumerateObject().ToDictionary(p => p.Name, p => p.Value),
                            typeof(Dictionary<string, JsonElement>)),
                        context);

                    return EquivalencyResult.AssertionCompleted;

                case JsonValueKind.Array:
                    nestedValidator.RecursivelyAssertEquality(
                        new Comparands(subject.EnumerateArray(), expectation.EnumerateArray(),
                            typeof(IEnumerable<JsonElement>)),
                        context);

                    return EquivalencyResult.AssertionCompleted;

                case JsonValueKind.String:
                    nestedValidator.RecursivelyAssertEquality(
                        new Comparands(subject.GetString(), expectation.GetString(), typeof(string)),
                        context);

                    return EquivalencyResult.AssertionCompleted;

                case JsonValueKind.Number:
                    nestedValidator.RecursivelyAssertEquality(
                        new Comparands(subject.GetRawText(), expectation.GetRawText(), typeof(string)),
                        context);

                    return EquivalencyResult.AssertionCompleted;

                case var unknownValueKind:
                    throw new ArgumentOutOfRangeException($"Unknown value kind '{unknownValueKind}'.");
            }
        }
    }
}
