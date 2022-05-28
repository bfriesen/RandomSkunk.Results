namespace RandomSkunk.Results.Json;

internal sealed class ExtendedErrorJsonConverter : JsonConverter<ExtendedError>
{
#pragma warning disable IDE1006 // Naming Styles
    private static readonly JsonEncodedText Message = JsonEncodedText.Encode("Message");
    private static readonly JsonEncodedText StackTrace = JsonEncodedText.Encode("StackTrace");
    private static readonly JsonEncodedText ErrorCode = JsonEncodedText.Encode("ErrorCode");
    private static readonly JsonEncodedText Identifier = JsonEncodedText.Encode("Identifier");
    private static readonly JsonEncodedText Type = JsonEncodedText.Encode("Type");
    private static readonly JsonEncodedText InnerError = JsonEncodedText.Encode("InnerError");
#pragma warning restore IDE1006 // Naming Styles

    public override ExtendedError? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var builder = new ExtendedErrorBuilder();

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Unexcepted end when reading JSON.");

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            ReadValue(ref reader, builder, options);

        if (reader.TokenType != JsonTokenType.EndObject)
            throw new JsonException("Unexcepted end when reading JSON.");

        return builder.Build();
    }

    public override void Write(Utf8JsonWriter writer, ExtendedError value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteExtendedError(writer, value, options);
        writer.WriteEndObject();
    }

    private static void ReadValue(ref Utf8JsonReader reader, ExtendedErrorBuilder builder, JsonSerializerOptions options)
    {
        if (TryReadStringProperty(ref reader, Message, out var propertyValue))
        {
            builder.Message = propertyValue;
        }
        else if (TryReadStringProperty(ref reader, StackTrace, out propertyValue))
        {
            builder.StackTrace = propertyValue;
        }
        else if (reader.ValueTextEquals(ErrorCode.EncodedUtf8Bytes))
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.Null)
                builder.ErrorCode = reader.GetInt32();
        }
        else if (TryReadStringProperty(ref reader, Identifier, out propertyValue))
        {
            builder.Identifier = propertyValue;
        }
        else if (TryReadStringProperty(ref reader, Type, out propertyValue))
        {
            builder.Type = propertyValue;
        }
        else
        {
            if (reader.ValueTextEquals(InnerError.EncodedUtf8Bytes))
            {
                builder.InnerError = JsonSerializer.Deserialize<ExtendedError>(ref reader, options);
            }
            else
            {
                var key = reader.GetString()!;
                reader.Read();
                var deserializedValue = JsonSerializer.Deserialize(ref reader, typeof(object), options);
                if (deserializedValue is null)
                    return;
                builder.Extensions[key] = deserializedValue;
            }
        }
    }

    private static void WriteExtendedError(Utf8JsonWriter writer, ExtendedError value, JsonSerializerOptions options)
    {
        if (value.Message != null)
            writer.WriteString(Message, value.Message);

        if (value.StackTrace != null)
            writer.WriteString(StackTrace, value.StackTrace);

        if (value.ErrorCode != null)
            writer.WriteNumber(ErrorCode, value.ErrorCode.Value);

        if (value.Identifier != null)
            writer.WriteString(Identifier, value.Identifier);

        if (value.Type != null)
            writer.WriteString(Type, value.Type);

        if (value.InnerError != null)
        {
            writer.WritePropertyName(InnerError);
            JsonSerializer.Serialize(writer, value.InnerError, value.InnerError.GetType(), options);
        }

        foreach (var extendedData in value.Extensions)
        {
            if (extendedData.Value is null) continue;
            writer.WritePropertyName(extendedData.Key);
            JsonSerializer.Serialize(writer, extendedData.Value, extendedData.Value.GetType(), options);
        }
    }

    private static bool TryReadStringProperty(ref Utf8JsonReader reader, JsonEncodedText propertyName, out string? value)
    {
        if (!reader.ValueTextEquals(propertyName.EncodedUtf8Bytes))
        {
            value = default;
            return false;
        }

        reader.Read();
        value = reader.GetString()!;
        return true;
    }

    private class ExtendedErrorBuilder
    {
        private Dictionary<string, object>? _extensions;

        public string? Message { get; set; }

        public string? StackTrace { get; set; }

        public int? ErrorCode { get; set; }

        public string? Identifier { get; set; }

        public string? Type { get; set; }

        public Error? InnerError { get; set; }

        public Dictionary<string, object> Extensions => _extensions ??= new Dictionary<string, object>(StringComparer.Ordinal);

        public ExtendedError Build() =>
            new(Message, Type, Extensions)
            {
                StackTrace = StackTrace,
                ErrorCode = ErrorCode,
                Identifier = Identifier,
                InnerError = InnerError,
            };
    }
}
