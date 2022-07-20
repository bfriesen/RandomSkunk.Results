namespace RandomSkunk.Results;

internal static class Errors
{
    public const string BadRequestMessage = "The operation cannot be processed due to a client error.";
    public const string NotFoundMessage = "The target resource cannot be found.";
    public const string GoneMessage = "The target resource is no longer available.";
    public const string InternalServerErrorMessage = "An unexpected condition prevented the operation from completing successfully.";
    public const string NotImplementedMessage = "The operation is not implemented.";
    public const string BadGatewayMessage = "The response from the upstream service was invalid.";
    public const string GatewayTimeoutMessage = "The operation from the upstream service timed out.";

    public static Error BadRequest(
        string errorMessage = BadRequestMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? BadRequestMessage, $"{nameof(BadRequest)}Error", true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.BadRequest,
        };

    public static Error NotFound(
        string errorMessage = NotFoundMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? NotFoundMessage, $"{nameof(NotFound)}Error", true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.NotFound,
        };

    public static Error Gone(
        string errorMessage = GoneMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? GoneMessage, $"{nameof(Gone)}Error", true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.Gone,
        };

    public static Error InternalServerError(
        string errorMessage = GoneMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? InternalServerErrorMessage, $"{nameof(InternalServerError)}Error", true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.InternalServerError,
        };

    public static Error NotImplemented(
        string errorMessage = NotImplementedMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? NotImplementedMessage, $"{nameof(NotImplemented)}Error", true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.NotImplemented,
        };

    public static Error BadGateway(
        string errorMessage = BadGatewayMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? BadGatewayMessage, $"{nameof(BadGateway)}Error", true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.BadGateway,
        };

    public static Error GatewayTimeout(
        string errorMessage = GatewayTimeoutMessage,
        string? errorIdentifier = null) =>
        new(errorMessage ?? GatewayTimeoutMessage, $"{nameof(GatewayTimeout)}Error", true)
        {
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.GatewayTimeout,
        };
}
