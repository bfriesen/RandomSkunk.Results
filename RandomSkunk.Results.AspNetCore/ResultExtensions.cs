using System.Threading.Tasks;

namespace RandomSkunk.Results.AspNetCore;

/// <summary>
/// Defines extension methods for converting result objects to action result objects.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result"/>.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="source"/> is a <c>Success</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static IActionResult ToActionResult(this Result source, int successStatusCode = 200)
    {
        if (successStatusCode < 200 || successStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(successStatusCode), successStatusCode, "Must be between 200 and 299 inclusive.");

        return source.Match<IActionResult>(
            success: () => new StatusCodeResult(successStatusCode),
            fail: error => new ObjectResult(error.GetProblemDetails()) { StatusCode = error.ErrorCode ?? 500 });
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="source"/> is a <c>Success</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static IActionResult ToActionResult<T>(this Result<T> source, int successStatusCode = 200)
    {
        if (successStatusCode < 200 || successStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(successStatusCode), successStatusCode, "Must be between 200 and 299 inclusive.");

        return source.Match<IActionResult>(
            success: value => new ObjectResult(value) { StatusCode = successStatusCode },
            fail: error => new ObjectResult(error.GetProblemDetails()) { StatusCode = error.ErrorCode ?? 500 });
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="someStatusCode">The status code to use when <paramref name="source"/> is a <c>Some</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static IActionResult ToActionResult<T>(this Maybe<T> source, int someStatusCode = 200)
    {
        if (someStatusCode < 200 || someStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(someStatusCode), someStatusCode, "Must be between 200 and 299 inclusive.");

        return source.Match<IActionResult>(
            some: value => new ObjectResult(value) { StatusCode = someStatusCode },
            none: () => new NotFoundResult(),
            fail: error => new ObjectResult(error.GetProblemDetails()) { StatusCode = error.ErrorCode ?? 500 });
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result"/>.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="source"/> is a <c>Success</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static async Task<IActionResult> ToActionResult(this Task<Result> source, int successStatusCode = 200) =>
        (await source.ConfigureAwait(false)).ToActionResult(successStatusCode);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="source"/> is a <c>Success</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> source, int successStatusCode = 200) =>
        (await source.ConfigureAwait(false)).ToActionResult(successStatusCode);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="someStatusCode">The status code to use when <paramref name="source"/> is a <c>Some</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static async Task<IActionResult> ToActionResult<T>(this Task<Maybe<T>> source, int someStatusCode = 200) =>
        (await source.ConfigureAwait(false)).ToActionResult(someStatusCode);
}
