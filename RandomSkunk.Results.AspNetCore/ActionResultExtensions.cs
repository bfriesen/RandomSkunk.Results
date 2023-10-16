using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace RandomSkunk.Results.AspNetCore;

/// <summary>
/// Defines extension methods for converting results into <see cref="IActionResult"/> objects.
/// </summary>
public static class ActionResultExtensions
{
    private static readonly Func<Error, IActionResult> _defaultOnFail = error => error.GetActionResult();

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Result"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate and return if the result is <c>Success</c>.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onSuccess"/> function; otherwise the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onFail"/> function.</returns>
    public static IActionResult ToActionResult(
        this Result sourceResult,
        Func<IActionResult> onSuccess,
        Func<Error, IActionResult>? onFail = null)
    {
        if (onSuccess is null)
            throw new ArgumentNullException(nameof(onSuccess));

        onFail ??= _defaultOnFail;

        return sourceResult.Match(
            onSuccess: onSuccess,
            onFail: onFail);
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate and return if the result is <c>Success</c>.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onSuccess"/> function; otherwise the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onFail"/> function.</returns>
    public static IActionResult ToActionResult<T>(
        this Result<T> sourceResult,
        Func<T, IActionResult> onSuccess,
        Func<Error, IActionResult>? onFail = null)
    {
        if (onSuccess is null)
            throw new ArgumentNullException(nameof(onSuccess));

        onFail ??= _defaultOnFail;

        return sourceResult.Match(
            onSuccess: onSuccess,
            onFail: onFail);
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate and return if the result is <c>Success</c>.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onSuccess"/> function; otherwise the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onFail"/> function.</returns>
    public static IActionResult ToActionResult<T>(
        this Maybe<T> sourceResult,
        Func<T, IActionResult> onSuccess,
        Func<Error, IActionResult>? onFail = null)
    {
        if (onSuccess is null)
            throw new ArgumentNullException(nameof(onSuccess));

        onFail ??= _defaultOnFail;

        return sourceResult.Match(
            onSuccess: onSuccess,
            onNone: () => onFail(Errors.NoValue()),
            onFail: onFail);
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Result"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate and return if the result is <c>Success</c>.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onSuccess"/> function; otherwise the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onFail"/> function.</returns>
    public static async Task<IActionResult> ToActionResult(
        this Task<Result> sourceResult,
        Func<IActionResult> onSuccess,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToActionResult(onSuccess, onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate and return if the result is <c>Success</c>.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onSuccess"/> function; otherwise the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onFail"/> function.</returns>
    public static async Task<IActionResult> ToActionResult<T>(
        this Task<Result<T>> sourceResult,
        Func<T, IActionResult> onSuccess,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToActionResult(onSuccess, onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate and return if the result is <c>Success</c>.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onSuccess"/> function; otherwise the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onFail"/> function.</returns>
    public static async Task<IActionResult> ToActionResult<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, IActionResult> onSuccess,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToActionResult(onSuccess, onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Result"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="StatusCodeResult"/> with status code from
    ///     <paramref name="successStatusCode"/>; otherwise the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onFail"/> function.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static IActionResult ToActionResult(
        this Result sourceResult,
        int successStatusCode = 200,
        Func<Error, IActionResult>? onFail = null)
    {
        if (successStatusCode < 200 || successStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(successStatusCode), successStatusCode, "Must be between 200 and 299 inclusive.");

        return sourceResult.ToActionResult(
            () => new StatusCodeResult(successStatusCode),
            onFail);
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, an <see cref="ObjectResult"/> for its value; otherwise the
    ///     <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static IActionResult ToActionResult<T>(
        this Result<T> sourceResult,
        int successStatusCode = 200,
        Func<Error, IActionResult>? onFail = null)
    {
        if (successStatusCode < 200 || successStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(successStatusCode), successStatusCode, "Must be between 200 and 299 inclusive.");

        return sourceResult.ToActionResult(
            value => new ObjectResult(value) { StatusCode = successStatusCode },
            onFail);
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, an <see cref="ObjectResult"/> for its value; otherwise the
    ///     <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static IActionResult ToActionResult<T>(
        this Maybe<T> sourceResult,
        int successStatusCode = 200,
        Func<Error, IActionResult>? onFail = null)
    {
        if (successStatusCode < 200 || successStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(successStatusCode), successStatusCode, "Must be between 200 and 299 inclusive.");

        return sourceResult.ToActionResult(
            value => new ObjectResult(value) { StatusCode = successStatusCode },
            onFail);
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Result"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="StatusCodeResult"/> with status code from
    ///     <paramref name="successStatusCode"/>; otherwise the <see cref="IActionResult"/> returned by the
    ///     <paramref name="onFail"/> function.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static async Task<IActionResult> ToActionResult(
        this Task<Result> sourceResult,
        int successStatusCode = 200,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToActionResult(successStatusCode, onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, an <see cref="ObjectResult"/> for its value; otherwise the
    ///     <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static async Task<IActionResult> ToActionResult<T>(
        this Task<Result<T>> sourceResult,
        int successStatusCode = 200,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToActionResult(successStatusCode, onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that represents the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="sourceResult"/> is a <c>Success</c> result.
    ///     </param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, an <see cref="ObjectResult"/> for its value; otherwise the
    ///     <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="successStatusCode"/> is less than 200 or greater than
    ///     299.</exception>
    public static async Task<IActionResult> ToActionResult<T>(
        this Task<Maybe<T>> sourceResult,
        int successStatusCode = 200,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToActionResult(successStatusCode, onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that represents the source <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileContentResult"/> with file contents from its value;
    ///     otherwise the <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    public static IActionResult ToFileActionResult(
        this Result<byte[]> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<Error, IActionResult>? onFail = null)
    {
        if (string.IsNullOrEmpty(contentType))
            throw new ArgumentNullException(nameof(contentType));

        return sourceResult.ToActionResult(
            fileContents => new FileContentResult(fileContents, contentType) { FileDownloadName = fileDownloadName },
            onFail);
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that represents the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileContentResult"/> with file contents from its value;
    ///     otherwise the <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    public static IActionResult ToFileActionResult(
        this Maybe<byte[]> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<Error, IActionResult>? onFail = null)
    {
        if (string.IsNullOrEmpty(contentType))
            throw new ArgumentNullException(nameof(contentType));

        return sourceResult.ToActionResult(
            fileContents => new FileContentResult(fileContents, contentType) { FileDownloadName = fileDownloadName },
            onFail);
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that represents the source <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileContentResult"/> with file contents from its value;
    ///     otherwise the <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    public static async Task<IActionResult> ToFileActionResult(
        this Task<Result<byte[]>> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFileActionResult(contentType, fileDownloadName, onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that represents the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileContentResult"/> with file contents from its value;
    ///     otherwise the <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    public static async Task<IActionResult> ToFileActionResult(
        this Task<Maybe<byte[]>> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFileActionResult(contentType, fileDownloadName, onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that represents the source <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileStreamResult"/> with file stream from its value;
    ///     otherwise the <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    public static IActionResult ToFileActionResult(
        this Result<Stream> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<Error, IActionResult>? onFail = null)
    {
        if (string.IsNullOrEmpty(contentType))
            throw new ArgumentNullException(nameof(contentType));

        return sourceResult.ToActionResult(
            fileStream => new FileStreamResult(fileStream, contentType) { FileDownloadName = fileDownloadName },
            onFail);
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that represents the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileStreamResult"/> with file stream from its value;
    ///     otherwise the <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    public static IActionResult ToFileActionResult(
        this Maybe<Stream> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<Error, IActionResult>? onFail = null)
    {
        if (string.IsNullOrEmpty(contentType))
            throw new ArgumentNullException(nameof(contentType));

        return sourceResult.ToActionResult(
            fileStream => new FileStreamResult(fileStream, contentType) { FileDownloadName = fileDownloadName },
            onFail);
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that represents the source <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileStreamResult"/> with file stream from its value;
    ///     otherwise the <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    public static async Task<IActionResult> ToFileActionResult(
        this Task<Result<Stream>> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFileActionResult(contentType, fileDownloadName, onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for a file that represents the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="contentType">The Content-Type header of the response.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="FileStreamResult"/> with file stream from its value;
    ///     otherwise the <see cref="IActionResult"/> returned by the <paramref name="onFail"/> function.</returns>
    public static async Task<IActionResult> ToFileActionResult(
        this Task<Maybe<Stream>> sourceResult,
        string contentType,
        string? fileDownloadName = null,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFileActionResult(contentType, fileDownloadName, onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for JSON that represents the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="serializerSettings">The serializer settings to be used by the formatter.
    ///     <para>
    ///     When using <c>System.Text.Json</c>, this should be an instance of <see cref="JsonSerializerOptions"/>.
    ///     </para>
    ///     <para>
    ///     When using <c>Newtonsoft.Json</c>, this should be an instance of <c>JsonSerializerSettings</c>.
    ///     </para>
    /// </param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="JsonResult"/>; otherwise the <see cref="IActionResult"/>
    ///     returned by the <paramref name="onFail"/> function.</returns>
    public static IActionResult ToJsonActionResult<T>(
        this Result<T> sourceResult,
        object? serializerSettings = null,
        Func<Error, IActionResult>? onFail = null) =>
        sourceResult.ToActionResult(
            value => new JsonResult(value, serializerSettings),
            onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for JSON that represents the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="serializerSettings">The serializer settings to be used by the formatter.
    ///     <para>
    ///     When using <c>System.Text.Json</c>, this should be an instance of <see cref="JsonSerializerOptions"/>.
    ///     </para>
    ///     <para>
    ///     When using <c>Newtonsoft.Json</c>, this should be an instance of <c>JsonSerializerSettings</c>.
    ///     </para>
    /// </param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="JsonResult"/>; otherwise the <see cref="IActionResult"/>
    ///     returned by the <paramref name="onFail"/> function.</returns>
    public static IActionResult ToJsonActionResult<T>(
        this Maybe<T> sourceResult,
        object? serializerSettings = null,
        Func<Error, IActionResult>? onFail = null) =>
        sourceResult.ToActionResult(
            value => new JsonResult(value, serializerSettings),
            onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for JSON that represents the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="serializerSettings">The serializer settings to be used by the formatter.
    ///     <para>
    ///     When using <c>System.Text.Json</c>, this should be an instance of <see cref="JsonSerializerOptions"/>.
    ///     </para>
    ///     <para>
    ///     When using <c>Newtonsoft.Json</c>, this should be an instance of <c>JsonSerializerSettings</c>.
    ///     </para>
    /// </param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="JsonResult"/>; otherwise the <see cref="IActionResult"/>
    ///     returned by the <paramref name="onFail"/> function.</returns>
    public static async Task<IActionResult> ToJsonActionResult<T>(
        this Task<Result<T>> sourceResult,
        object? serializerSettings = null,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToJsonActionResult(serializerSettings, onFail);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> for JSON that represents the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="serializerSettings">The serializer settings to be used by the formatter.
    ///     <para>
    ///     When using <c>System.Text.Json</c>, this should be an instance of <see cref="JsonSerializerOptions"/>.
    ///     </para>
    ///     <para>
    ///     When using <c>Newtonsoft.Json</c>, this should be an instance of <c>JsonSerializerSettings</c>.
    ///     </para>
    /// </param>
    /// <param name="onFail">An optional function that is used to get an <see cref="IActionResult"/> from an <see cref="Error"/>.
    ///     If <see langword="null"/>, a function that returns an <see cref="ObjectResult"/> for a <see cref="ProblemDetails"/>
    ///     describing the non-success result is used instead.</param>
    /// <returns>If the source result is <c>Success</c>, a <see cref="JsonResult"/>; otherwise the <see cref="IActionResult"/>
    ///     returned by the <paramref name="onFail"/> function.</returns>
    public static async Task<IActionResult> ToJsonActionResult<T>(
        this Task<Maybe<T>> sourceResult,
        object? serializerSettings = null,
        Func<Error, IActionResult>? onFail = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToJsonActionResult(serializerSettings, onFail);
}
