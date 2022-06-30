using System.Runtime.InteropServices;

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
    /// <returns>A new <see cref="Error"/> object.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="exception"/> is <see langword="null"/>.
    /// </exception>
    public static Error FromException(
        Exception exception,
        string? message = null,
        int? errorCode = null,
        string? identifier = null)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        if (string.IsNullOrWhiteSpace(message))
            message = exception.Message;
        else
            message += Environment.NewLine + exception.Message;

        if (errorCode is null && exception is ExternalException externalException)
            errorCode = externalException.ErrorCode;

        Error? innerError = null;
        if (exception.InnerException != null)
            innerError = FromException(exception.InnerException);

        return new Error(message, exception.GetType().Name)
        {
            StackTrace = exception.StackTrace,
            ErrorCode = errorCode,
            Identifier = identifier,
            InnerError = innerError,
        };
    }

    /// <inheritdoc/>
    public sealed override string ToString() => ToString(this, null);

    /// <summary>
    /// Returns a string that represents the specified error.
    /// </summary>
    /// <param name="error">The error to get a string representation of.</param>
    /// <param name="indention">The indention that begins each line.</param>
    /// <returns>A string that represents the specified error.</returns>
    protected static string ToString(Error error, string? indention)
    {
        var sb = new StringBuilder();
        sb.Append(indention).Append(error.Type).Append(": ").Append(Indent(error.Message.TrimEnd(), indention));
        if (error.Identifier is not null)
            sb.AppendLine().Append(indention).Append("Identifier: ").Append(error.Identifier);
        if (error.ErrorCode is not null)
            sb.AppendLine().Append(indention).Append("Error Code: ").Append(error.ErrorCode);
        error.PrintAdditionalProperties(sb, indention);
        if (error.StackTrace is not null)
            sb.AppendLine().Append(indention).Append("Stack Trace:").AppendLine().Append(Indent(error.StackTrace.TrimEnd(), indention, indention));
        if (error.InnerError is not null)
            sb.AppendLine().Append(indention).Append("Inner Error:").AppendLine().Append(ToString(error.InnerError, indention + "   "));
        return sb.ToString();
    }

    /// <summary>
    /// When overridden in an inherited class, appends additional properties of the derived error class to the
    /// <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="sb">The <see cref="StringBuilder"/> to append to.</param>
    /// <param name="indention">Leading whitespace for the beginning of each line.</param>
    protected virtual void PrintAdditionalProperties(StringBuilder sb, string? indention)
    {
    }

    private static string Indent(string value, string? indention, string? firstLineIndentation = null)
    {
        if (indention is null)
            return value;

        return firstLineIndentation + value.Replace("\n", "\n" + indention);
    }

    private string GetDebuggerDisplay() => $"{Type}: \"{Message}\"";
}
