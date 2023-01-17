using System.IO;
using System.Threading.Tasks;

namespace RandomSkunk.Results.AspNetCore;

/// <summary>
/// Defines extension methods for converting result objects to action result objects.
/// </summary>
public static class ActionResultExtensions
{
    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static IActionResult ToActionResult(
        this Result sourceResult,
        int successStatusCode = 200,
        Func<int, int>? getHttpStatusCode = null)
    {
        if (successStatusCode < 200 || successStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(successStatusCode), successStatusCode, "Must be between 200 and 299 inclusive.");

        return sourceResult.Match(
            onSuccess: () => new StatusCodeResult(successStatusCode),
            onFail: error => GetFailActionResult(error, getHttpStatusCode));
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static IActionResult ToActionResult<T>(
        this Result<T> sourceResult,
        int successStatusCode = 200,
        Func<int, int>? getHttpStatusCode = null)
    {
        if (successStatusCode < 200 || successStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(successStatusCode), successStatusCode, "Must be between 200 and 299 inclusive.");

        return sourceResult.Match(
            onSuccess: value => new ObjectResult(value) { StatusCode = successStatusCode },
            onFail: error => GetFailActionResult(error, getHttpStatusCode));
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static IActionResult ToActionResult<T>(
        this Maybe<T> sourceResult,
        int successStatusCode = 200,
        Func<int, int>? getHttpStatusCode = null)
    {
        if (successStatusCode < 200 || successStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(successStatusCode), successStatusCode, "Must be between 200 and 299 inclusive.");

        return sourceResult.Match(
            onSuccess: value => new ObjectResult(value) { StatusCode = successStatusCode },
            onNone: () => GetNoneActionResult(getHttpStatusCode),
            onFail: error => GetFailActionResult(error, getHttpStatusCode));
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static async Task<IActionResult> ToActionResult(
        this Task<Result> sourceResult,
        int successStatusCode = 200,
        Func<int, int>? getHttpStatusCode = null) =>
        (await sourceResult.ConfigureAwait(false)).ToActionResult(successStatusCode, getHttpStatusCode);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static async Task<IActionResult> ToActionResult<T>(
        this Task<Result<T>> sourceResult,
        int successStatusCode = 200,
        Func<int, int>? getHttpStatusCode = null) =>
        (await sourceResult.ConfigureAwait(false)).ToActionResult(successStatusCode, getHttpStatusCode);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static async Task<IActionResult> ToActionResult<T>(
        this Task<Maybe<T>> sourceResult,
        int successStatusCode = 200,
        Func<int, int>? getHttpStatusCode = null) =>
        (await sourceResult.ConfigureAwait(false)).ToActionResult(successStatusCode, getHttpStatusCode);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileContentResult"/> with file contents from its value;
    ///     otherwise an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/> describing the non-success result.
    ///     </returns>
    public static IActionResult ToFileActionResult(
        this Result<byte[]> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<int, int>? getHttpStatusCode = null)
    {
        if (string.IsNullOrEmpty(contentType))
            throw new ArgumentNullException(nameof(contentType));

        return sourceResult.Match(
            onSuccess: fileContents => new FileContentResult(fileContents, contentType) { FileDownloadName = fileDownloadName },
            onFail: error => GetFailActionResult(error, getHttpStatusCode));
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileContentResult"/> with file contents from its value;
    ///     otherwise an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/> describing the non-success result.
    ///     </returns>
    public static IActionResult ToFileActionResult(
        this Maybe<byte[]> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<int, int>? getHttpStatusCode = null)
    {
        if (string.IsNullOrEmpty(contentType))
            throw new ArgumentNullException(nameof(contentType));

        return sourceResult.Match(
            onSuccess: fileContents => new FileContentResult(fileContents, contentType) { FileDownloadName = fileDownloadName },
            onNone: () => GetNoneActionResult(getHttpStatusCode),
            onFail: error => GetFailActionResult(error, getHttpStatusCode));
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileContentResult"/> with file contents from its value;
    ///     otherwise an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/> describing the non-success result.
    ///     </returns>
    public static async Task<IActionResult> ToFileActionResult(
        this Task<Result<byte[]>> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<int, int>? getHttpStatusCode = null) =>
        (await sourceResult).ToFileActionResult(contentType, fileDownloadName, getHttpStatusCode);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileContentResult"/> with file contents from its value;
    ///     otherwise an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/> describing the non-success result.
    ///     </returns>
    public static async Task<IActionResult> ToFileActionResult(
        this Task<Maybe<byte[]>> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<int, int>? getHttpStatusCode = null) =>
        (await sourceResult).ToFileActionResult(contentType, fileDownloadName, getHttpStatusCode);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileStreamResult"/> with file stream from its value;
    ///     otherwise an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/> describing the non-success result.
    ///     </returns>
    public static IActionResult ToFileActionResult(
        this Result<Stream> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<int, int>? getHttpStatusCode = null)
    {
        if (string.IsNullOrEmpty(contentType))
            throw new ArgumentNullException(nameof(contentType));

        return sourceResult.Match(
            onSuccess: fileStream => new FileStreamResult(fileStream, contentType) { FileDownloadName = fileDownloadName },
            onFail: error => GetFailActionResult(error, getHttpStatusCode));
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileStreamResult"/> with file stream from its value;
    ///     otherwise an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/> describing the non-success result.
    ///     </returns>
    public static IActionResult ToFileActionResult(
        this Maybe<Stream> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<int, int>? getHttpStatusCode = null)
    {
        if (string.IsNullOrEmpty(contentType))
            throw new ArgumentNullException(nameof(contentType));

        return sourceResult.Match(
            onSuccess: fileStream => new FileStreamResult(fileStream, contentType) { FileDownloadName = fileDownloadName },
            onNone: () => GetNoneActionResult(getHttpStatusCode),
            onFail: error => GetFailActionResult(error, getHttpStatusCode));
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileStreamResult"/> with file stream from its value;
    ///     otherwise an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/> describing the non-success result.
    ///     </returns>
    public static async Task<IActionResult> ToFileActionResult(
        this Task<Result<Stream>> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<int, int>? getHttpStatusCode = null) =>
        (await sourceResult).ToFileActionResult(contentType, fileDownloadName, getHttpStatusCode);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="getHttpStatusCode">An optional function that is used to get an HTTP status code from an
    ///     <see cref="Error.ErrorCode"/>. If <see langword="null"/> or not provided, then the following function is used:
    ///     <code>errorCode => Math.Abs(errorCode) % 1000</code>
    /// </param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileStreamResult"/> with file stream from its value;
    ///     otherwise an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/> describing the non-success result.
    ///     </returns>
    public static async Task<IActionResult> ToFileActionResult(
        this Task<Maybe<Stream>> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<int, int>? getHttpStatusCode = null) =>
        (await sourceResult).ToFileActionResult(contentType, fileDownloadName, getHttpStatusCode);

    private static IActionResult GetFailActionResult(Error error, Func<int, int>? getHttpStatusCode)
    {
        var httpStatusCode = error.GetHttpStatusCode(getHttpStatusCode);

        return new ObjectResult(error.GetProblemDetails(getHttpStatusCode: getHttpStatusCode))
        {
            StatusCode = httpStatusCode ?? ErrorCodes.InternalServerError,
        };
    }

    private static IActionResult GetNoneActionResult(Func<int, int>? getHttpStatusCode)
    {
        var error = Errors.NoValue();
        return GetFailActionResult(error, getHttpStatusCode);
    }
}
