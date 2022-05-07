using System.Runtime.Serialization;

namespace RandomSkunk.Results;

/// <summary>
/// The exception that is thrown when accessing a result's <c>Error</c> or <c>Value</c> property
/// and the result is in an invalid state to do so.
/// </summary>
/// <remarks>
/// In order to access a result's <c>Error</c> property, its <c>IsFail</c> property must be true.
/// In order to access a result's <c>Value</c> property, its <c>IsSuccess</c> or <c>IsSome</c>
/// property must be true. Otherwise, in each case, this exception is thrown.
/// </remarks>
public class InvalidStateException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStateException"/> class.
    /// </summary>
    public InvalidStateException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStateException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public InvalidStateException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStateException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference if no inner
    /// exception is specified.
    /// </param>
    public InvalidStateException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStateException"/> class with serialized data.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected InvalidStateException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
