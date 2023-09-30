using Xunit.Abstractions;

namespace Serilog.Sinks.Humio.Tests;

public static class TestOutputHelperExtensions
{
    public static void Dump(this ITestOutputHelper @this, object? value, string header)
    {
        @this.WriteLine("################# " + header + " #################");
        @this.WriteLine($"{value}");
        @this.WriteLine("################# /" + header + " #################");
    }
}
