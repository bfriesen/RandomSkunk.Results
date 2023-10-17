using System.Collections.Concurrent;

namespace RandomSkunk.Results;

/// <summary>
/// A class that defines error codes.
/// </summary>
public static class ErrorCodes
{
    /// <summary>
    /// Indicates that the operation cannot be processed due to a client error.
    /// </summary>
    public const int BadRequest = 400;

    /// <summary>
    /// Indicates that the client request was not completed because it lacks valid authentication credentials for the requested
    /// resource.
    /// </summary>
    public const int Unauthorized = 401;

    /// <summary>
    /// Indicates that the server understands the request but refuses to authorize it.
    /// </summary>
    public const int Forbidden = 403;

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
    /// Indicates that the operation was canceled.
    /// </summary>
    public const int Canceled = -400;

    /// <summary>
    /// Indicates that the result has no value.
    /// </summary>
    public const int NoValue = -404;

    /// <summary>
    /// Indicates that the error represents a caught exception. The inner error contains information about the exception.
    /// </summary>
    public const int CaughtException = -500;

    /// <summary>
    /// Indicates that the error represents an unexpected null value.
    /// </summary>
    public const int UnexpectedNullValue = -1404;

    private static readonly ConcurrentDictionary<int, string> _descriptions = new(GetErrorCodes(typeof(ErrorCodes)));

    /// <summary>
    /// Gets a description of an error code.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <returns>The description of the error code if the error code is known; otherwise a regular string representation of the
    ///     error code value.</returns>
    public static string? GetDescription(int errorCode)
    {
        if (_descriptions.TryGetValue(errorCode, out var description))
            return description;

        return errorCode.ToString();
    }

    /// <summary>
    /// Registers all error codes defined in the specified type. Each <c>public const int</c> field defined by the type is
    /// registered as an error code, able to have its description retrieved with the <see cref="GetDescription"/> method.
    /// </summary>
    /// <param name="errorCodesType">A type that defines error codes.</param>
    public static void RegisterErrorCodes(Type errorCodesType)
    {
        var errorCodes = GetErrorCodes(errorCodesType);

        foreach (var errorCode in errorCodes)
            _descriptions.AddOrUpdate(errorCode.Key, errorCode.Value, (k, e) => errorCode.Value);
    }

    internal static bool TryGetDescription(int errorCode, [NotNullWhen(true)]out string? description)
    {
        return _descriptions.TryGetValue(errorCode, out description);
    }

    private static IEnumerable<KeyValuePair<int, string>> GetErrorCodes(Type errorCodesType) =>
        errorCodesType
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral && f.FieldType == typeof(int))
            .Select(f =>
            {
                var value = (int)f.GetValue(null)!;
                return new KeyValuePair<int, string>(value, $"{value} ({Format.AsSentenceCase(f.Name)})");
            });
}
