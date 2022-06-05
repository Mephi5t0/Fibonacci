using System.Text.Json.Serialization;

namespace View.Errors
{
    public sealed class Error
    {
        [JsonPropertyName("Message")]
        public string Message { get; set; }

        [JsonPropertyName("Code")]
        public string Code { get; set; }
    }
}