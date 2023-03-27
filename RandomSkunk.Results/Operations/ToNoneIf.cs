using static RandomSkunk.Results.AwaitSettings;

namespace RandomSkunk.Results;

/// <content> Defines the <c>ToNoneIf</c> method. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Gets a <c>None</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <param name="predicate">The delegate that determines whether to return a <c>None</c> result.</param>
    /// <returns>A <c>None</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public Maybe<T> ToNoneIf(Func<T, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (_outcome == Outcome.Success && predicate(_value!))
            return None;

        return this;
    }
}

/// <content> Defines the <c>ToNoneIf</c> extension method. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Gets a <c>None</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">The delegate that determines whether to return a <c>None</c> result.</param>
    /// <returns>A <c>None</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Maybe<T>> ToNoneIf<T>(this Task<Maybe<T>> sourceResult, Func<T, bool> predicate) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToNoneIf(predicate);
}
