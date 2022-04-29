namespace RandomSkunk.Results;

/// <summary>
/// Defines an error that occurred in an operation.
/// </summary>
public sealed class Error : IEquatable<Error>
{
    private static string _defaultMessage = "Error";

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="stackTrace">The optional stack trace.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="message"/> is <see langword="null"/>.
    /// </exception>
    public Error(string? message = null, string? stackTrace = null, int? errorCode = null, string? identifier = null)
    {
        Message = message ?? DefaultMessage;
        StackTrace = stackTrace;
        ErrorCode = errorCode;
        Identifier = identifier;
    }

    /// <summary>
    /// Gets or sets the default error message. This value is used when creating a <c>fail</c>
    /// result and an error message is not specified.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// When setting the property, if the value is <see langword="null"/>.
    /// </exception>
    public static string DefaultMessage
    {
        get => _defaultMessage;
        set => _defaultMessage = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the optional stack trace.
    /// </summary>
    public string? StackTrace { get; }

    /// <summary>
    /// Gets the optional error code.
    /// </summary>
    public int? ErrorCode { get; }

    /// <summary>
    /// Gets the optional identifier of the error.
    /// </summary>
    public string? Identifier { get; }

    /// <summary>
    /// Creates an <see cref="Error"/> object from the specified <see cref="Exception"/>.
    /// </summary>
    /// <param name="exception">The exception to create the error from.</param>
    /// <param name="messagePrefix">An optional prefix for the exception message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A new <see cref="Error"/> object.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="exception"/> is <see langword="null"/>.
    /// </exception>
    public static Error FromException(
        Exception exception,
        string? messagePrefix = null,
        int? errorCode = null,
        string? identifier = null)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));
        messagePrefix ??= $"{exception.GetType().Name}: ";

        return new Error($"{messagePrefix}{exception.Message}", exception.StackTrace, errorCode, identifier);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder(Message);
        if (Identifier != null)
            sb.AppendLine().Append($"Identifier: {Identifier}");
        if (ErrorCode != null)
            sb.AppendLine().Append($"Error code: {ErrorCode}");
        if (StackTrace != null)
            sb.AppendLine().AppendLine("Stack trace:").Append(StackTrace);
        return sb.ToString();
    }

    /// <inheritdoc/>
    public bool Equals(Error? other) =>
        other != null
            && Message == other.Message
            && StackTrace == other.StackTrace
            && ErrorCode == other.ErrorCode
            && Identifier == other.Identifier;

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Error other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 1812228152;
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Message);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(StackTrace ?? string.Empty);
        hashCode = (hashCode * -1521134295) + ErrorCode.GetHashCode();
        hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(Identifier ?? string.Empty);
        return hashCode;
    }
}
