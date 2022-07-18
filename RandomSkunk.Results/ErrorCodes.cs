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
    /// Equivalent to HTTP status 400. <see cref="BadRequest"/> indicates that the request could not be understood by the server.
    /// <see cref="BadRequest"/> is sent when no other error is applicable, or if the exact error is unknown or does not have its
    /// own error code.
    /// </summary>
    public const int BadRequest = 400;

    /// <summary>
    /// Equivalent to HTTP status 404. <see cref="NotFound"/> indicates that the requested resource does not exist.
    /// </summary>
    public const int NotFound = 404;

    /// <summary>
    /// Equivalent to HTTP status 410. <see cref="Gone"/> indicates that the requested resource is no longer available.
    /// </summary>
    public const int Gone = 410;

    /// <summary>
    /// Equivalent to HTTP status 500. <see cref="InternalServerError"/> indicates that a generic error has occurred on the
    /// server.
    /// </summary>
    public const int InternalServerError = 500;

    /// <summary>
    /// Equivalent to HTTP status 501. <see cref="NotImplemented"/> indicates that the server does not support the requested
    /// function.
    /// </summary>
    public const int NotImplemented = 501;

    /// <summary>
    /// Equivalent to HTTP status 502. <see cref="BadGateway"/> indicates that an intermediate proxy server received a bad
    /// response from another proxy or the origin server.
    /// </summary>
    public const int BadGateway = 502;

    /// <summary>
    /// Equivalent to HTTP status 503. <see cref="ServiceUnavailable"/> indicates that the server is temporarily unavailable,
    /// usually due to high load or maintenance.
    /// </summary>
    public const int ServiceUnavailable = 503;

    /// <summary>
    /// Equivalent to HTTP status 504. <see cref="GatewayTimeout"/> indicates that an intermediate proxy server timed out while
    /// waiting for a response from another proxy or the origin server.
    /// </summary>
    public const int GatewayTimeout = 504;
}
