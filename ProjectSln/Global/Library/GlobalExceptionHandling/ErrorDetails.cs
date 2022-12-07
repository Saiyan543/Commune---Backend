using System.Text.Json;

namespace Main.Global.Library.GlobalExceptionHandling
{
    public sealed class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}