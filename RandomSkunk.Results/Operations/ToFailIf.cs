namespace RandomSkunk.Results;

/// <content> Defines the <c>ToFailIf</c> method. </content>
public partial struct Result
{
    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <param name="predicate">The delegate that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">An optional function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public Result ToFailIf(Func<bool> predicate, Func<Error>? getError = null)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (_outcome == Outcome.Success && predicate())
            return Fail(getError?.Invoke());

        return this;
    }
}

/// <content> Defines the <c>ToFailIf</c> method. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <param name="predicate">The delegate that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">An optional function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public Result<T> ToFailIf(Func<T, bool> predicate, Func<T, Error>? getError = null)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (_outcome == Outcome.Success && predicate(_value!))
            return Fail(getError?.Invoke(_value!));

        return this;
    }
}

/// <content> Defines the <c>ToFailIf</c> method. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <param name="predicate">The delegate that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">An optional function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public Maybe<T> ToFailIf(Func<T, bool> predicate, Func<T, Error>? getError = null)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (_outcome == Outcome.Success && predicate(_value!))
            return Fail(getError?.Invoke(_value!));

        return this;
    }
}

/// <content> Defines the <c>ToFailIf</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">The delegate that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">An optional function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Result> ToFailIf(this Task<Result> sourceResult, Func<bool> predicate, Func<Error>? getError = null) =>
        (await sourceResult.ConfigureAwait(false)).ToFailIf(predicate, getError);

    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">The delegate that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">An optional function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Result<T>> ToFailIf<T>(this Task<Result<T>> sourceResult, Func<T, bool> predicate, Func<T, Error>? getError = null) =>
        (await sourceResult.ConfigureAwait(false)).ToFailIf(predicate, getError);

    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">The delegate that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">An optional function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Maybe<T>> ToFailIf<T>(this Task<Maybe<T>> sourceResult, Func<T, bool> predicate, Func<T, Error>? getError = null) =>
        (await sourceResult.ConfigureAwait(false)).ToFailIf(predicate, getError);
}