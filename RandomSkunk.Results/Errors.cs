namespace RandomSkunk.Results;

/// <summary>
/// A class that creates <see cref="Error"/> instances.
/// </summary>
public static class Errors
{
    internal const string BadRequestTitle = "Bad Request";
    internal const string BadRequestMessage = "The operation cannot be processed due to a client error.";

    internal const string UnauthorizedTitle = "Unauthorized";
    internal const string UnauthorizedMessage = "The operation was not completed because no credentials or invalid credentials were provided.";

    internal const string ForbiddenTitle = "Forbidden";
    internal const string ForbiddenMessage = "The operation was not completed because the provided credentials, while valid, do not grant sufficient privileges.";

    internal const string NotFoundTitle = "Not Found";
    internal const string NotFoundMessage = "The target resource cannot be found.";

    internal const string GoneTitle = "Gone";
    internal const string GoneMessage = "The target resource is no longer available.";

    internal const string InternalServerErrorTitle = "Internal Server Error";
    internal const string InternalServerErrorMessage = "An unexpected condition prevented the operation from completing successfully.";

    internal const string NotImplementedTitle = "Not Implemented";
    internal const string NotImplementedMessage = "The operation is not implemented.";

    internal const string BadGatewayTitle = "Bad Gateway";
    internal const string BadGatewayMessage = "The response from the upstream service was invalid.";

    internal const string GatewayTimeoutTitle = "Gateway Timeout";
    internal const string GatewayTimeoutMessage = "The operation from the upstream service timed out.";

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation cannot be processed due to a client error.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="setStackTrace">Whether to set the stack trace of the error to the current location.</param>
    /// <returns>A Bad Request error.</returns>
    [StackTraceHidden]
    public static Error BadRequest(
        string errorMessage = BadRequestMessage,
        string? errorIdentifier = null,
        bool setStackTrace = true) =>
        new(errorMessage ?? BadRequestMessage, BadRequestTitle, setStackTrace)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.BadRequest,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the client request was not completed because it lacks valid authentication
    /// credentials for the requested resource.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="setStackTrace">Whether to set the stack trace of the error to the current location.</param>
    /// <returns>An Unauthorized error.</returns>
    [StackTraceHidden]
    public static Error Unauthorized(
        string errorMessage = UnauthorizedMessage,
        string? errorIdentifier = null,
        bool setStackTrace = true) =>
        new(errorMessage ?? UnauthorizedMessage, UnauthorizedTitle, setStackTrace)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.Unauthorized,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the server understands the request but refuses to authorize it.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="setStackTrace">Whether to set the stack trace of the error to the current location.</param>
    /// <returns>A Forbidden error.</returns>
    [StackTraceHidden]
    public static Error Forbidden(
        string errorMessage = ForbiddenMessage,
        string? errorIdentifier = null,
        bool setStackTrace = true) =>
        new(errorMessage ?? ForbiddenMessage, ForbiddenTitle, setStackTrace)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.Forbidden,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the target resource cannot be found.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="setStackTrace">Whether to set the stack trace of the error to the current location.</param>
    /// <returns>A Not Found error.</returns>
    [StackTraceHidden]
    public static Error NotFound(
        string errorMessage = NotFoundMessage,
        string? errorIdentifier = null,
        bool setStackTrace = true) =>
        new(errorMessage ?? NotFoundMessage, NotFoundTitle, setStackTrace)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.NotFound,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the target resource is no longer available.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="setStackTrace">Whether to set the stack trace of the error to the current location.</param>
    /// <returns>A Gone error.</returns>
    [StackTraceHidden]
    public static Error Gone(
        string errorMessage = GoneMessage,
        string? errorIdentifier = null,
        bool setStackTrace = true) =>
        new(errorMessage ?? GoneMessage, GoneTitle, setStackTrace)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.Gone,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that an unexpected condition prevented the operation from completing
    /// successfully.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="setStackTrace">Whether to set the stack trace of the error to the current location.</param>
    /// <returns>An Internal Server Error error.</returns>
    [StackTraceHidden]
    public static Error InternalServerError(
        string errorMessage = InternalServerErrorMessage,
        string? errorIdentifier = null,
        bool setStackTrace = true) =>
        new(errorMessage ?? InternalServerErrorMessage, InternalServerErrorTitle, setStackTrace)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.InternalServerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation is not implemented.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="setStackTrace">Whether to set the stack trace of the error to the current location.</param>
    /// <returns>A Not Implemented error.</returns>
    [StackTraceHidden]
    public static Error NotImplemented(
        string errorMessage = NotImplementedMessage,
        string? errorIdentifier = null,
        bool setStackTrace = true) =>
        new(errorMessage ?? NotImplementedMessage, NotImplementedTitle, setStackTrace)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.NotImplemented,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the response from the upstream service was invalid.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="setStackTrace">Whether to set the stack trace of the error to the current location.</param>
    /// <returns>A Bad Gateway error.</returns>
    [StackTraceHidden]
    public static Error BadGateway(
        string errorMessage = BadGatewayMessage,
        string? errorIdentifier = null,
        bool setStackTrace = true) =>
        new(errorMessage ?? BadGatewayMessage, BadGatewayTitle, setStackTrace)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.BadGateway,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation from the upstream service timed out.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="setStackTrace">Whether to set the stack trace of the error to the current location.</param>
    /// <returns>A Gateway Timeout error.</returns>
    [StackTraceHidden]
    public static Error GatewayTimeout(
        string errorMessage = GatewayTimeoutMessage,
        string? errorIdentifier = null,
        bool setStackTrace = true) =>
        new(errorMessage ?? GatewayTimeoutMessage, GatewayTimeoutTitle, setStackTrace)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.GatewayTimeout,
        };

    [StackTraceHidden]
    internal static Error NoneResult() =>
        new("This error represents a lack of a value (i.e. None).", "Result is None", true)
        {
            ErrorCode = ErrorCodes.NoneResult,
        };
}
