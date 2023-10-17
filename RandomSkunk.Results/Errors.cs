namespace RandomSkunk.Results;

/// <summary>
/// A class that creates <see cref="Error"/> instances.
/// </summary>
public static class Errors
{
    private const string _badRequestTitle = "Bad Request";
    private const string _badRequestMessage = "The operation cannot be processed due to a client error.";

    private const string _unauthorizedTitle = "Unauthorized";
    private const string _unauthorizedMessage = "The operation was not completed because no credentials or invalid credentials were provided.";

    private const string _forbiddenTitle = "Forbidden";
    private const string _forbiddenMessage = "The operation was not completed because the provided credentials, while valid, do not grant sufficient privileges.";

    private const string _notFoundTitle = "Not Found";
    private const string _notFoundMessage = "The target resource cannot be found.";

    private const string _goneTitle = "Gone";
    private const string _goneMessage = "The target resource is no longer available.";

    private const string _internalServerErrorTitle = "Internal Server Error";
    private const string _internalServerErrorMessage = "An unexpected condition prevented the operation from completing successfully.";

    private const string _notImplementedTitle = "Not Implemented";
    private const string _notImplementedMessage = "The operation is not implemented.";

    private const string _badGatewayTitle = "Bad Gateway";
    private const string _badGatewayMessage = "The response from the upstream service was invalid.";

    private const string _gatewayTimeoutTitle = "Gateway Timeout";
    private const string _gatewayTimeoutMessage = "The operation from the upstream service timed out.";

    private const string _unexpectedNullValueTitle = "Unexpected Null Value";
    private const string _unexpectedNullValueMessage = "The value was null when it was not expected to be.";

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation cannot be processed due to a client error.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Bad Request error.</param>
    /// <returns>A Bad Request error.</returns>
    public static Error BadRequest(
        string errorMessage = _badRequestMessage,
        string? errorIdentifier = null,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? _badRequestMessage,
            Title = _badRequestTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.BadRequest,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the client request was not completed because it lacks valid authentication
    /// credentials for the requested resource.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Unauthorized error.</param>
    /// <returns>An Unauthorized error.</returns>
    public static Error Unauthorized(
        string errorMessage = _unauthorizedMessage,
        string? errorIdentifier = null,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? _unauthorizedMessage,
            Title = _unauthorizedTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.Unauthorized,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the server understands the request but refuses to authorize it.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Forbidden error.</param>
    /// <returns>A Forbidden error.</returns>
    public static Error Forbidden(
        string errorMessage = _forbiddenMessage,
        string? errorIdentifier = null,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? _forbiddenMessage,
            Title = _forbiddenTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.Forbidden,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the target resource cannot be found.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Not Found error.</param>
    /// <returns>A Not Found error.</returns>
    public static Error NotFound(
        string errorMessage = _notFoundMessage,
        string? errorIdentifier = null,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? _notFoundMessage,
            Title = _notFoundTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.NotFound,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the target resource is no longer available.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Gone error.</param>
    /// <returns>A Gone error.</returns>
    public static Error Gone(
        string errorMessage = _goneMessage,
        string? errorIdentifier = null,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? _goneMessage,
            Title = _goneTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.Gone,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that an unexpected condition prevented the operation from completing
    /// successfully.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Internal Server Error error.</param>
    /// <returns>An Internal Server Error error.</returns>
    public static Error InternalServerError(
        string errorMessage = _internalServerErrorMessage,
        string? errorIdentifier = null,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? _internalServerErrorMessage,
            Title = _internalServerErrorTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.InternalServerError,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation is not implemented.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Not Implemented error.</param>
    /// <returns>A Not Implemented error.</returns>
    public static Error NotImplemented(
        string errorMessage = _notImplementedMessage,
        string? errorIdentifier = null,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? _notImplementedMessage,
            Title = _notImplementedTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.NotImplemented,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the response from the upstream service was invalid.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Bad Gateway error.</param>
    /// <returns>A Bad Gateway error.</returns>
    public static Error BadGateway(
        string errorMessage = _badGatewayMessage,
        string? errorIdentifier = null,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? _badGatewayMessage,
            Title = _badGatewayTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.BadGateway,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation from the upstream service timed out.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Gateway Timeout error.</param>
    /// <returns>A Gateway Timeout error.</returns>
    public static Error GatewayTimeout(
        string errorMessage = _gatewayTimeoutMessage,
        string? errorIdentifier = null,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? _gatewayTimeoutMessage,
            Title = _gatewayTimeoutTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.GatewayTimeout,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that a value was <see langword="null"/> when it was not expected to be.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Unexpected Null Value error.</param>
    /// <returns>A Unexpected Null Value error.</returns>
    public static Error UnexpectedNullValue(
        string errorMessage = _unexpectedNullValueMessage,
        string? errorIdentifier = null,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? _unexpectedNullValueMessage,
            Title = _unexpectedNullValueTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.UnexpectedNullValue,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the result has no value.
    /// </summary>
    /// <returns>A No Value error.</returns>
    public static Error NoValue() =>
        new()
        {
            Message = "This error indicates that the result has no value.",
            Title = "Result Has No Value",
            ErrorCode = ErrorCodes.NoValue,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation was canceled.
    /// </summary>
    /// <param name="taskCanceledException">The exception that was thrown indicating that the operation was canceled.
    ///     </param>
    /// <returns>A Canceled error.</returns>
    public static Error Canceled(TaskCanceledException? taskCanceledException = null)
    {
        const string title = "Canceled";
        const string message = "The operation was canceled.";

        if (taskCanceledException is null)
        {
            return new()
            {
                Title = title,
                Message = message,
                ErrorCode = ErrorCodes.Canceled,
            };
        }

        return Error.FromException(
            taskCanceledException,
            title: title,
            message: message + " See InnerError for details.",
            errorCode: ErrorCodes.Canceled);
    }
}
