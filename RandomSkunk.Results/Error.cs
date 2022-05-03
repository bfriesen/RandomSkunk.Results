using System.Reflection;

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
    /// <param name="message">
    /// The optional error message. If <see langword="null"/>, then the value of
    /// <see cref="DefaultMessage"/> is used instead.
    /// </param>
    /// <param name="stackTrace">The optional stack trace.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="type">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="System.Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="message"/> is <see langword="null"/>.
    /// </exception>
    public Error(
        string? message = null,
        string? stackTrace = null,
        int? errorCode = null,
        string? identifier = null,
        string? type = null)
    {
        Message = message ?? DefaultMessage;
        StackTrace = stackTrace;
        ErrorCode = errorCode;
        Identifier = identifier;
        Type = type ?? GetType().Name;
    }

    /// <summary>
    /// Gets or sets the default error message, to be used when an error message is not specified.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// If the property is to <see langword="null"/>.
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
    /// Gets the type of the error.
    /// </summary>
    public string Type { get; }

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

        var exceptionMessage = $"{exception.GetType().Name}: {exception.Message}";

        if (string.IsNullOrWhiteSpace(message))
            message = exceptionMessage;
        else
            message += Environment.NewLine + exceptionMessage;

        return new Error(message, exception.StackTrace, errorCode, identifier);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder(Type).Append(": ").Append(Message);
        if (Identifier != null)
            sb.AppendLine().Append("Identifier: ").Append(Identifier);
        if (ErrorCode != null)
            sb.AppendLine().Append("Error code: ").Append(ErrorCode);
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
