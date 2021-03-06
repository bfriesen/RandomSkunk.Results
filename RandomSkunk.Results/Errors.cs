namespace RandomSkunk.Results;

/// <summary>
/// A class that creates <see cref="Error"/> instances.
/// </summary>
public static class Errors
{
    internal const string BadRequestTitle = "Bad Request";
    internal const string BadRequestMessage = "The operation cannot be processed due to a client error.";

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
    /// <returns>A Bad Request error.</returns>
    [StackTraceHidden]
    public static Error BadRequest(
        string errorMessage = BadRequestMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? BadRequestMessage, BadRequestTitle, true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.BadRequest,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the target resource cannot be found.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A Not Found error.</returns>
    [StackTraceHidden]
    public static Error NotFound(
        string errorMessage = NotFoundMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? NotFoundMessage, NotFoundTitle, true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.NotFound,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the target resource is no longer available.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A error.</returns>
    [StackTraceHidden]
    public static Error Gone(
        string errorMessage = GoneMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? GoneMessage, GoneTitle, true)
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
    /// <returns>A error.</returns>
    [StackTraceHidden]
    public static Error InternalServerError(
        string errorMessage = InternalServerErrorMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? InternalServerErrorMessage, InternalServerErrorTitle, true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.InternalServerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation is not implemented.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A error.</returns>
    [StackTraceHidden]
    public static Error NotImplemented(
        string errorMessage = NotImplementedMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? NotImplementedMessage, NotImplementedTitle, true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.NotImplemented,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the response from the upstream service was invalid.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A error.</returns>
    [StackTraceHidden]
    public static Error BadGateway(
        string errorMessage = BadGatewayMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? BadGatewayMessage, BadGatewayTitle, true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.BadGateway,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation from the upstream service timed out.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A error.</returns>
    [StackTraceHidden]
    public static Error GatewayTimeout(
        string errorMessage = GatewayTimeoutMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? GatewayTimeoutMessage, GatewayTimeoutTitle, true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.GatewayTimeout,
        };

    [StackTraceHidden]
    internal static Error ResultIsNone() =>
        new("This error represents a lack of a value (i.e. None).", "Result is None", true)
        {
            ErrorCode = ErrorCodes.ResultIsNone,
        };
}
