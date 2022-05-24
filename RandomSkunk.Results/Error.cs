namespace RandomSkunk.Results;

/// <summary>
/// Defines an error.
/// </summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public record class Error
{
    private static readonly Lazy<Error> _defaultError = new(() => new Error());

    private static string _defaultMessage = "An error occurred.";

    private readonly string _message;
    private readonly string _type;

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="message">
    /// The error message. If <see langword="null"/>, then the value of <see cref="DefaultMessage"/> is used instead.
    /// </param>
    /// <param name="type">
    /// The type of the error. If <see langword="null"/>, then the name of the error type is used instead.
    /// </param>
    public Error(string? message = null, string? type = null)
    {
        _message = message ?? _defaultMessage;
        _type = type ?? GetType().Name;
    }

    /// <summary>
    /// Gets or sets the default error message, to be used when an error message is not specified.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// If the property is set to <see langword="null"/>.
    /// </exception>
    public static string DefaultMessage
    {
        get => _defaultMessage;
        set => _defaultMessage = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message
    {
        get => _message;
        init => _message = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets the optional stack trace.
    /// </summary>
    public string? StackTrace { get; init; }

    /// <summary>
    /// Gets the optional error code.
    /// </summary>
    public int? ErrorCode { get; init; }

    /// <summary>
    /// Gets the optional identifier of the error.
    /// </summary>
    public string? Identifier { get; init; }

    /// <summary>
    /// Gets the type of the error.
    /// </summary>
    public string Type
    {
        get => _type;
        init => _type = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets the optional <see cref="Error"/> instance that caused the current error.
    /// </summary>
    [JsonConverter(typeof(InnerErrorJsonConverter))]
    public Error? InnerError { get; init; }

    internal static Error DefaultError => _defaultError.Value;

    /// <summary>
    /// Creates an <see cref="Error"/> object from the specified <see cref="Exception"/>.
    /// </summary>
    /// <param name="exception">The exception to create the error from.</param>
    /// <param name="message">The error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="type">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="System.Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <returns>A new <see cref="Error"/> object.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="exception"/> is <see langword="null"/>.
    /// </exception>
    public static Error FromException(
        Exception exception,
        string? message = null,
        int? errorCode = null,
        string? identifier = null,
        string? type = null,
        Error? innerError = null)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var exceptionMessage = $"{exception.GetType().Name}: {exception.Message}";

        if (string.IsNullOrWhiteSpace(message))
            message = exceptionMessage;
        else
            message += Environment.NewLine + exceptionMessage;

        return new Error(message, type)
        {
            StackTrace = exception.StackTrace,
            ErrorCode = errorCode,
            Identifier = identifier,
            InnerError = innerError,
        };
    }

    /// <inheritdoc/>
    public override string ToString() => ToString(string.Empty);

    /// <summary>
    /// When overridden in an inherited class, appends additional properties of the derived error class to the
    /// <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="sb">The <see cref="StringBuilder"/> to append to.</param>
    /// <param name="indention">The leading spaces for the beginning of each line.</param>
    protected virtual void ToStringAppendAdditionalProperties(StringBuilder sb, string indention)
    {
    }

    private static string Indent(string value, string indention, string? firstLineIndentation = null)
    {
        if (indention is null)
            return value;

        return firstLineIndentation + value.Replace("\n", "\n" + indention);
    }

    private string ToString(string indention)
    {
        var sb = new StringBuilder();
        sb.Append(indention).Append(Type).Append(": ").Append(Indent(Message, indention));
        if (Identifier is not null)
            sb.AppendLine().Append(indention).Append("Identifier: ").Append(Identifier);
        if (ErrorCode is not null)
            sb.AppendLine().Append(indention).Append("Error Code: ").Append(ErrorCode);
        ToStringAppendAdditionalProperties(sb, indention);
        if (StackTrace is not null)
            sb.AppendLine().Append(indention).Append("Stack Trace:").AppendLine().Append(Indent(StackTrace, indention, indention));
        if (InnerError is not null)
            sb.AppendLine().Append(indention).Append("Inner Error:").AppendLine().Append(InnerError.ToString(indention + "   "));
        return sb.ToString();
    }

    private string GetDebuggerDisplay() => $"{Type}: \"{Message}\"";
}
