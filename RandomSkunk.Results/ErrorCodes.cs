namespace RandomSkunk.Results;

/// <summary>
/// A class that defines error codes.
/// </summary>
/// <remarks>
/// This class is abstract to allow inheritors to add additional error codes.
/// </remarks>
public abstract class ErrorCodes
{
    /// <summary>
    /// Indicates that the operation cannot be processed due to a client error.
    /// </summary>
    public const int BadRequest = 400;

    /// <summary>
    /// Indicates that the target resource cannot be found.
    /// </summary>
    public const int NotFound = 404;

    /// <summary>
    /// Indicates that the target resource is no longer available.
    /// </summary>
    public const int Gone = 410;

    /// <summary>
    /// Indicates that an unexpected condition prevented the operation from completing successfully.
    /// </summary>
    public const int InternalServerError = 500;

    /// <summary>
    /// Indicates that the operation is not implemented.
    /// </summary>
    public const int NotImplemented = 501;

    /// <summary>
    /// Indicates that the response from the upstream service was invalid.
    /// </summary>
    public const int BadGateway = 502;

    /// <summary>
    /// Indicates that the operation from the upstream service timed out.
    /// </summary>
    public const int GatewayTimeout = 504;

    /// <summary>
    /// Indicates that the error represents a <see cref="Maybe{T}"/> <c>None</c> result.
    /// </summary>
    public const int ResultIsNone = -410;

    /// <summary>
    /// Indicates that the error represents a caught exception. The inner error contains information about the exception.
    /// </summary>
    public const int CaughtException = -500;
}
