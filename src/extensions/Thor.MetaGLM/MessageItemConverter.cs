using System.Text.Json;
using System.Text.Json.Serialization;
using Thor.MetaGLM.Models.RequestModels;
using Thor.MetaGLM.Models.RequestModels.ImageToTextModels;

namespace Thor.MetaGLM
{
    public class MessageItemConverter : JsonConverter<MessageItem>
    {
        public override MessageItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Implement deserialization logic if needed
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, MessageItem value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Serialize all Properties except 'content' for ImageToTextMessageItem
            foreach (var prop in value.GetType().GetProperties())
            {
                if (value is ImageToTextMessageItem && prop.Name.Equals("content", StringComparison.OrdinalIgnoreCase))
                {
                    // Skip serializing 'content' for ImageToTextMessageItem
                    continue;
                }

                var propValue = prop.GetValue(value);
                writer.WritePropertyName(prop.Name);
                JsonSerializer.Serialize(writer, propValue, prop.PropertyType, options);
            }

            writer.WriteEndObject();
        }
    }

}
