namespace RandomSkunk.Results;

/// <summary>
/// Defines an error that can have extension properties. This error is useful for JSON deserializing an error that could
/// represent an unknown custom error type - any unknown JSON properties are deserialized into the <see cref="Extensions"/>
/// property and are available from the <see cref="TryGet{T}(string, out T)"/> and
/// <see cref="TryGet{T}(string, JsonSerializerOptions, out T)"/> methods.
/// </summary>
[JsonConverter(typeof(ExtendedErrorJsonConverter))]
public sealed record class ExtendedError : Error
{
    private static IReadOnlyDictionary<string, object>? _emptyExtensions;

    private readonly IReadOnlyDictionary<string, object> _extensions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendedError"/> class.
    /// </summary>
    /// <param name="message">The error message. If <see langword="null"/>, then the value of <see cref="Error.DefaultMessage"/>
    ///     is used instead.</param>
    /// <param name="type">The type of the error. If <see langword="null"/>, then the name of the error type is used instead.
    ///     </param>
    /// <param name="extensions">Additional properties not found in the <see cref="Error"/> class.</param>
    public ExtendedError(string? message = null, string? type = null, IReadOnlyDictionary<string, object>? extensions = null)
        : base(message, type ?? "Error") =>
        _extensions = extensions ?? (_emptyExtensions ??= new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()));

    /// <summary>
    /// Gets additional properties not found in the <see cref="Error"/> class.
    /// </summary>
    public IReadOnlyDictionary<string, object> Extensions
    {
        get => _extensions;
        init => _extensions = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets the extension property with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the extension property.</typeparam>
    /// <param name="key">The name of the extension property.</param>
    /// <param name="options">JSON serialization options used to deserialize to the desired type when the actual value is a
    ///     <see cref="JsonElement"/>.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found
    ///     and is of a valid type; otherwise, the default value for the type of the value parameter. This parameter is passed
    ///     uninitialized.</param>
    /// <returns><see langword="true"/> if the <see cref="ExtendedError"/> contains an extension property with the specified key
    ///     that is of type <typeparamref name="T"/> or convertible to type <typeparamref name="T"/>; otherwise,
    ///     <see langword="false"/>.</returns>
    public bool TryGet<T>(string key, JsonSerializerOptions? options, [NotNullWhen(true)] out T? value)
    {
        if (Extensions.TryGetValue(key, out var obj) && obj != null)
        {
            if (obj is T t)
            {
                value = t;
                return true;
            }

            if (obj is JsonElement jsonElement)
            {
                value = jsonElement.Deserialize<T>(options);
                return value != null;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(obj.GetType()))
            {
                value = (T?)converter.ConvertFrom(obj);
                return value != null;
            }
            else
            {
                converter = TypeDescriptor.GetConverter(obj);
                if (converter.CanConvertTo(typeof(T)))
                {
                    value = (T?)converter.ConvertTo(obj, typeof(T));
                    return value != null;
                }
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Gets the extension property with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the extension property.</typeparam>
    /// <param name="key">The name of the extension property.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found
    ///     and is of a valid type; otherwise, the default value for the type of the value parameter. This parameter is passed
    ///     uninitialized.</param>
    /// <returns><see langword="true"/> if the <see cref="ExtendedError"/> contains an extension property with the specified key
    ///     that is of type <typeparamref name="T"/> or convertible to type <typeparamref name="T"/>; otherwise,
    ///     <see langword="false"/>.</returns>
    public bool TryGet<T>(string key, [NotNullWhen(true)] out T? value) =>
        TryGet(key, null, out value);

    /// <inheritdoc/>
    protected override void PrintAdditionalProperties(StringBuilder sb, string? indention, bool includeStackTrace)
    {
        foreach (var extension in Extensions)
        {
            sb.AppendLine().Append(indention).Append(extension.Key).Append(": ")
                .Append(extension.Value is Error error
                    ? error.ToString(includeStackTrace)
                    : extension.Value?.ToString());
        }
    }
}
