namespace RandomSkunk.Results.Json;

internal sealed class InnerErrorJsonConverter : JsonConverter<Error>
{
    public override Error? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        JsonSerializer.Deserialize<ExtendedError>(ref reader, options);

    public override void Write(Utf8JsonWriter writer, Error value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
}
