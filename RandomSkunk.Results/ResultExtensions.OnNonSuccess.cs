namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>OnNonSuccess</c> and <c>OnNonSuccessAsync</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Invokes the <paramref name="onNonSuccess"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccess">A callback function to invoke if this is a <c>non-Success</c> result.</param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> that is returned if this is a <c>None</c> result; otherwise
    /// this parameter is ignored. If <see langword="null"/> (and applicable), a function that returns an error with message
    /// "Not Found" and error code 404 is used instead.
    /// </param>
    /// <returns>The current result.</returns>
    public static TResult OnNonSuccess<TResult>(
        this TResult sourceResult,
        Action<Error> onNonSuccess,
        Func<Error>? getNoneError = null)
        where TResult : IResult
    {
        if (onNonSuccess is null) throw new ArgumentNullException(nameof(onNonSuccess));

        if (!sourceResult.IsSuccess)
        {
            var error = sourceResult.GetNonSuccessError(getNoneError);
            onNonSuccess(error);
        }

        return sourceResult;
    }

    /// <summary>
    /// Invokes the <paramref name="onNonSuccess"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccess">A callback function to invoke if this is a <c>non-Success</c> result.</param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> that is returned if this is a <c>None</c> result; otherwise
    /// this parameter is ignored. If <see langword="null"/> (and applicable), a function that returns an error with message
    /// "Not Found" and error code 404 is used instead.
    /// </param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnNonSuccessAsync<TResult>(
        this TResult sourceResult,
        Func<Error, Task> onNonSuccess,
        Func<Error>? getNoneError = null)
        where TResult : IResult
    {
        if (onNonSuccess is null) throw new ArgumentNullException(nameof(onNonSuccess));

        if (!sourceResult.IsSuccess)
        {
            var error = sourceResult.GetNonSuccessError(getNoneError);
            await onNonSuccess(error).ConfigureAwait(false);
        }

        return sourceResult;
    }
}
