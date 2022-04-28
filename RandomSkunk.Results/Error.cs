namespace RandomSkunk.Results;

/// <summary>
/// Defines an error that occurred in an operation.
/// </summary>
public sealed class Error : IEquatable<Error>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="message">The error message of the failed operation.</param>
    /// <param name="errorCode">The optional error code of the failed operation.</param>
    /// <param name="stackTrace">The optional stack trace of the failed operation.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="message"/> is <see langword="null"/>.
    /// </exception>
    public Error(string message, string? errorCode, string? stackTrace)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        ErrorCode = errorCode;
        StackTrace = stackTrace;
    }

    /// <summary>
    /// Gets the error message of the failed operation.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the optional error code of the failed operation.
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// Gets the optional stack trace of the failed operation.
    /// </summary>
    public string? StackTrace { get; }

    /// <inheritdoc/>
    public bool Equals(Error? other) =>
        other != null
            && Message == other.Message
            && ErrorCode == other.ErrorCode
            && StackTrace == other.StackTrace;

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Error other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 1812228152;
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Message);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(ErrorCode ?? string.Empty);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(StackTrace ?? string.Empty);
        return hashCode;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder(Message);
        if (ErrorCode != null)
            sb.Append(Environment.NewLine).Append($"Error code: {ErrorCode}");
        if (StackTrace != null)
            sb.Append(Environment.NewLine).AppendLine("Stack trace:").Append(StackTrace);
        return sb.ToString();
    }
}
