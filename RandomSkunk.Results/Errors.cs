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

    internal const string UnexpectedNullValueTitle = "Unexpected Null Value";
    internal const string UnexpectedNullValueMessage = "The value was null when it was not expected to be.";

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation cannot be processed due to a client error.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="isSensitive">Whether the error contains sensitive information.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Bad Request error.</param>
    /// <returns>A Bad Request error.</returns>
    public static Error BadRequest(
        string errorMessage = BadRequestMessage,
        string? errorIdentifier = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? BadRequestMessage,
            Title = BadRequestTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.BadRequest,
            IsSensitive = isSensitive,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the client request was not completed because it lacks valid authentication
    /// credentials for the requested resource.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="isSensitive">Whether the error contains sensitive information.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Unauthorized error.</param>
    /// <returns>An Unauthorized error.</returns>
    public static Error Unauthorized(
        string errorMessage = UnauthorizedMessage,
        string? errorIdentifier = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? UnauthorizedMessage,
            Title = UnauthorizedTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.Unauthorized,
            IsSensitive = isSensitive,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the server understands the request but refuses to authorize it.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="isSensitive">Whether the error contains sensitive information.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Forbidden error.</param>
    /// <returns>A Forbidden error.</returns>
    public static Error Forbidden(
        string errorMessage = ForbiddenMessage,
        string? errorIdentifier = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? ForbiddenMessage,
            Title = ForbiddenTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.Forbidden,
            IsSensitive = isSensitive,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the target resource cannot be found.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="isSensitive">Whether the error contains sensitive information.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Not Found error.</param>
    /// <returns>A Not Found error.</returns>
    public static Error NotFound(
        string errorMessage = NotFoundMessage,
        string? errorIdentifier = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? NotFoundMessage,
            Title = NotFoundTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.NotFound,
            IsSensitive = isSensitive,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the target resource is no longer available.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="isSensitive">Whether the error contains sensitive information.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Gone error.</param>
    /// <returns>A Gone error.</returns>
    public static Error Gone(
        string errorMessage = GoneMessage,
        string? errorIdentifier = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? GoneMessage,
            Title = GoneTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.Gone,
            IsSensitive = isSensitive,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that an unexpected condition prevented the operation from completing
    /// successfully.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="isSensitive">Whether the error contains sensitive information.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Internal Server Error error.</param>
    /// <returns>An Internal Server Error error.</returns>
    public static Error InternalServerError(
        string errorMessage = InternalServerErrorMessage,
        string? errorIdentifier = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? InternalServerErrorMessage,
            Title = InternalServerErrorTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.InternalServerError,
            IsSensitive = isSensitive,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation is not implemented.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="isSensitive">Whether the error contains sensitive information.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Not Implemented error.</param>
    /// <returns>A Not Implemented error.</returns>
    public static Error NotImplemented(
        string errorMessage = NotImplementedMessage,
        string? errorIdentifier = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? NotImplementedMessage,
            Title = NotImplementedTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.NotImplemented,
            IsSensitive = isSensitive,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the response from the upstream service was invalid.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="isSensitive">Whether the error contains sensitive information.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Bad Gateway error.</param>
    /// <returns>A Bad Gateway error.</returns>
    public static Error BadGateway(
        string errorMessage = BadGatewayMessage,
        string? errorIdentifier = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? BadGatewayMessage,
            Title = BadGatewayTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.BadGateway,
            IsSensitive = isSensitive,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the operation from the upstream service timed out.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="isSensitive">Whether the error contains sensitive information.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Gateway Timeout error.</param>
    /// <returns>A Gateway Timeout error.</returns>
    public static Error GatewayTimeout(
        string errorMessage = GatewayTimeoutMessage,
        string? errorIdentifier = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? GatewayTimeoutMessage,
            Title = GatewayTimeoutTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.GatewayTimeout,
            IsSensitive = isSensitive,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that a value was <see langword="null"/> when it was not expected to be.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="isSensitive">Whether the error contains sensitive information.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional <see cref="Error"/> instance that caused the Unexpected Null Value error.</param>
    /// <returns>A Unexpected Null Value error.</returns>
    public static Error UnexpectedNullValue(
        string errorMessage = UnexpectedNullValueMessage,
        string? errorIdentifier = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? UnexpectedNullValueMessage,
            Title = UnexpectedNullValueTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.UnexpectedNullValue,
            IsSensitive = isSensitive,
            Extensions = extensions!,
            InnerError = innerError,
        };

    /// <summary>
    /// Creates an <see cref="Error"/> indicating that the result has no value.
    /// </summary>
    /// <returns>A No Value error.</returns>
    /// <remarks>
    /// This error is to used to indicate that a <c>Fail</c> <see cref="Result{T}"/> is equivalent to
    /// <see cref="Maybe{T}.None"/>.
    /// </remarks>
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
