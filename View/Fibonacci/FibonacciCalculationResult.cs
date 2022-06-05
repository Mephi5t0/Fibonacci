using System.Text.Json.Serialization;

namespace View.Fibonacci
{
    public sealed class FibonacciCalculationResult
    {
        [JsonPropertyName("NextNumber")]
        public long NextNumber { get; set; }
    }
}