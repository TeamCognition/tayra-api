using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cog.Core
{
    public class ToStringJsonConverter : JsonConverter<object>
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}