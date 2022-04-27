namespace RandomSkunk.Results;

/// <summary>
/// Defines an error that occurred in an operation.
/// </summary>
public sealed class Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="message">The error message of the failed operation.</param>
    /// <param name="errorCode">The optional error code of the failed operation.</param>
    /// <param name="stackTrace">The optional stack trace of the failed operation.</param>
    public Error(string message, int? errorCode, string? stackTrace)
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
    public int? ErrorCode { get; }

    /// <summary>
    /// Gets the optional stack trace of the failed operation.
    /// </summary>
    public string? StackTrace { get; }
}
