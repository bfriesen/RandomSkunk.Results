using System.Threading.Tasks;

namespace RandomSkunk.Results.AspNetCore;

/// <summary>
/// Defines extension methods for results.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result"/>.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static IActionResult ToActionResult(this Result source) =>
        source.Match<IActionResult>(
            success: () => new OkResult(),
            fail: error => new ObjectResult(error.GetProblemDetails()) { StatusCode = error.ErrorCode ?? 500 });

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static IActionResult ToActionResult<T>(this Result<T> source) =>
        source.Match<IActionResult>(
            success: value => new OkObjectResult(value),
            fail: error => new ObjectResult(error.GetProblemDetails()) { StatusCode = error.ErrorCode ?? 500 });

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static IActionResult ToActionResult<T>(this Maybe<T> source) =>
        source.Match<IActionResult>(
            some: value => new OkObjectResult(value),
            none: () => new NotFoundResult(),
            fail: error => new ObjectResult(error.GetProblemDetails()) { StatusCode = error.ErrorCode ?? 500 });

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result"/>.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static async Task<IActionResult> ToActionResult(this Task<Result> source) =>
        (await source.ConfigureAwait(false)).ToActionResult();

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> source) =>
        (await source.ConfigureAwait(false)).ToActionResult();

    /// <summary>
    /// Gets an <see cref="IActionResult"/> that is equivalent to the source <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The equivalent action result object.</returns>
    public static async Task<IActionResult> ToActionResult<T>(this Task<Maybe<T>> source) =>
        (await source.ConfigureAwait(false)).ToActionResult();
}
