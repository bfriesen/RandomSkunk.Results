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
    /// <param name="source">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="source"/> is a <c>Success</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="successStatusCode"/> is less than 200 or greater than 299.
    /// </exception>
    public static IActionResult ToActionResult(this Result source, int successStatusCode = 200)
    {
        if (successStatusCode < 200 || successStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(successStatusCode), successStatusCode, "Must be between 200 and 299 inclusive.");

        return source.Match<IActionResult>(
            onSuccess: () => new StatusCodeResult(successStatusCode),
            onFail: error => new ObjectResult(error.GetProblemDetails()) { StatusCode = error.ErrorCode ?? 500 });
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="source"/> is a <c>Success</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="successStatusCode"/> is less than 200 or greater than 299.
    /// </exception>
    public static IActionResult ToActionResult<T>(this Result<T> source, int successStatusCode = 200)
    {
        if (successStatusCode < 200 || successStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(successStatusCode), successStatusCode, "Must be between 200 and 299 inclusive.");

        return source.Match<IActionResult>(
            onSuccess: value => new ObjectResult(value) { StatusCode = successStatusCode },
            onFail: error => new ObjectResult(error.GetProblemDetails()) { StatusCode = error.ErrorCode ?? 500 });
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="source"/> is a <c>Success</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="successStatusCode"/> is less than 200 or greater than 299.
    /// </exception>
    public static IActionResult ToActionResult<T>(this Maybe<T> source, int successStatusCode = 200)
    {
        if (successStatusCode < 200 || successStatusCode > 299)
            throw new ArgumentOutOfRangeException(nameof(successStatusCode), successStatusCode, "Must be between 200 and 299 inclusive.");

        return source.Match<IActionResult>(
            onSuccess: value => new ObjectResult(value) { StatusCode = successStatusCode },
            onNone: () => new NotFoundResult(),
            onFail: error => new ObjectResult(error.GetProblemDetails()) { StatusCode = error.ErrorCode ?? 500 });
    }

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result"/>.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="source"/> is a <c>Success</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="successStatusCode"/> is less than 200 or greater than 299.
    /// </exception>
    public static async Task<IActionResult> ToActionResult(this Task<Result> source, int successStatusCode = 200) =>
        (await source.ConfigureAwait(false)).ToActionResult(successStatusCode);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="source"/> is a <c>Success</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="successStatusCode"/> is less than 200 or greater than 299.
    /// </exception>
    public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> source, int successStatusCode = 200) =>
        (await source.ConfigureAwait(false)).ToActionResult(successStatusCode);

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="successStatusCode">The status code to use when <paramref name="source"/> is a <c>Success</c> result.</param>
    /// <returns>The equivalent action result object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="successStatusCode"/> is less than 200 or greater than 299.
    /// </exception>
    public static async Task<IActionResult> ToActionResult<T>(this Task<Maybe<T>> source, int successStatusCode = 200) =>
        (await source.ConfigureAwait(false)).ToActionResult(successStatusCode);
}
