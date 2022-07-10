using Customers.Domain.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Customers.Api.Converters
{
    public class EmailJsonConverter : JsonConverter<Email>
    {
        public override Email Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType is JsonTokenType.Null ? default(Email) : new Email(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, Email value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
