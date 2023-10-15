using System;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ApartmentPanel.Core.Services
{
    public class TypeMappingConverter<TType, TImplementation> : JsonConverter<TType>
      where TImplementation : TType
    {
        public override TType Read(
          ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            JsonSerializer.Deserialize<TImplementation>(ref reader, options);

        public override void Write(
          Utf8JsonWriter writer, TType value, JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, (TImplementation)value, options);
    }
}
