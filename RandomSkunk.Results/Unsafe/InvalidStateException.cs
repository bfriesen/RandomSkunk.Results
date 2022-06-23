namespace RandomSkunk.Results.Unsafe;

/// <summary>
/// The exception that is thrown when accessing a result's <c>Error</c> or <c>Value</c> property
/// and the result is in an invalid state to do so.
/// </summary>
/// <remarks>
/// In order to access a result's <c>Error</c> property, its <c>IsFail</c> property must be true.
/// In order to access a result's <c>Value</c> property, its <c>IsSuccess</c> or <c>IsSuccess</c>
/// property must be true. Otherwise, in each case, this exception is thrown.
/// </remarks>
public class InvalidStateException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStateException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="error">
    /// The error that is the cause of the current exception, or a null reference if no error is
    /// specified.
    /// </param>
    public InvalidStateException(string message, Error? error = null)
        : base(message)
    {
        Error = error;
    }

    /// <summary>
    /// Gets the error that is the cause of the current exception.
    /// </summary>
    public Error? Error { get; }
}
