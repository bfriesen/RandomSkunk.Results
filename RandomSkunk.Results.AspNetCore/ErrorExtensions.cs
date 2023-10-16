namespace RandomSkunk.Results.AspNetCore;

/// <summary>
/// Defines extension methods for the <see cref="Error"/> class.
/// </summary>
public static class ErrorExtensions
{
    // If the value of this field is changed, be sure to update doc comments that mention the old implementation.
    private static readonly Func<int, int> _defaultGetHttpStatusCode =
        errorCode => Math.Abs(errorCode) % 1000;

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Error"/>.
    /// </summary>
    /// <param name="sourceError">The source error.</param>
    /// <param name="type">A URI reference [RFC3986] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type (e.g., using HTML
    ///     [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be "about:blank".</param>
    /// <param name="instance">A URI reference that identifies the specific occurrence of the problem. It may or may not yield
    ///     further information if dereferenced.</param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    ///     This function discards the sign of the number and all but the last three digits of the number are used. For example,
    ///     passing -123456 returns 456.</param>
    /// <returns>An <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/> describing the error.</returns>
    public static IActionResult GetActionResult(
        this Error sourceError,
        string? type = null,
        string? instance = null,
        Func<int, int>? getHttpStatusCode = null)
    {
        var httpStatusCode = sourceError.GetHttpStatusCode(getHttpStatusCode);

        return new ObjectResult(sourceError.GetProblemDetails(type, instance, getHttpStatusCode))
        {
            StatusCode = httpStatusCode ?? ErrorCodes.InternalServerError,
        };
    }

    /// <summary>
    /// Gets a <see cref="ProblemDetails"/> object that is equavalent to the error object.
    /// </summary>
    /// <param name="sourceError">The <see cref="Error"/> to create a <see cref="ProblemDetails"/> from.</param>
    /// <param name="type">A URI reference [RFC3986] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type (e.g., using HTML
    ///     [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be "about:blank".</param>
    /// <param name="instance">A URI reference that identifies the specific occurrence of the problem. It may or may not yield
    ///     further information if dereferenced.</param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from the
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    ///     This function discards the sign of the number and all but the last three digits of the number are used. For example,
    ///     passing -123456 returns 456.</param>
    /// <returns>The equivalent problem details object.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceError"/> is <see langword="null"/>.</exception>
    public static ProblemDetails GetProblemDetails(
        this Error sourceError,
        string? type = null,
        string? instance = null,
        Func<int, int>? getHttpStatusCode = null)
    {
        if (sourceError is null) throw new ArgumentNullException(nameof(sourceError));

        var httpStatusCode = sourceError.GetHttpStatusCode(getHttpStatusCode);

        var problemDetails = new ProblemDetails
        {
            Type = type,
            Title = sourceError.Title,
            Status = httpStatusCode,
            Detail = sourceError.Message,
            Instance = instance,
        };

        if (sourceError.ErrorCode is not null)
            problemDetails.Extensions["errorCode"] = sourceError.ErrorCode;

        if (sourceError.Identifier is not null)
            problemDetails.Extensions["errorIdentifier"] = sourceError.Identifier;

        if (sourceError.InnerError is not null)
            problemDetails.Extensions["errorInnerError"] = sourceError.InnerError;

        return problemDetails;
    }

    /// <summary>
    /// Gets an HTTP status code for the error based on the value of its <see cref="Error.ErrorCode"/>.
    /// </summary>
    /// <remarks>
    /// This method maps an error code to an HTTP status code according to the following function:
    /// <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// This function discards the sign of the number and all but the last three digits of the number are used. For example,
    /// passing -123456 returns 456.
    /// </remarks>
    /// <param name="sourceError">The <see cref="Error"/> to get an HTTP status code from.</param>
    /// <returns>The HTTP status code.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceError"/> is <see langword="null"/>.</exception>
    public static int? GetHttpStatusCode(this Error sourceError)
    {
        if (sourceError is null) throw new ArgumentNullException(nameof(sourceError));

        return sourceError.GetHttpStatusCode(null);
    }

    internal static int? GetHttpStatusCode(this Error sourceError, Func<int, int>? getHttpStatusCode)
    {
        if (!sourceError.ErrorCode.HasValue)
            return default;

        return (getHttpStatusCode ?? _defaultGetHttpStatusCode).Invoke(sourceError.ErrorCode.Value);
    }
}
