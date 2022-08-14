namespace RandomSkunk.Results;

/// <content> Defines the <c>WithError</c> method. </content>
public partial struct Result
{
    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, the current result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public Result WithError(Func<Error, Error> onFailGetError)
    {
        if (onFailGetError is null) throw new ArgumentNullException(nameof(onFailGetError));

        return _outcome == Outcome.Fail
            ? Fail(onFailGetError(GetError()))
            : this;
    }
}

/// <content> Defines the <c>WithError</c> method. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, the current result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public Result<T> WithError(Func<Error, Error> onFailGetError)
    {
        if (onFailGetError is null) throw new ArgumentNullException(nameof(onFailGetError));

        return _outcome == Outcome.Fail
            ? Fail(onFailGetError(GetError()))
            : this;
    }
}

/// <content> Defines the <c>WithError</c> method. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, the current result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public Maybe<T> WithError(Func<Error, Error> onFailGetError)
    {
        if (onFailGetError is null) throw new ArgumentNullException(nameof(onFailGetError));

        return _outcome == MaybeOutcome.Fail
            ? Fail(onFailGetError(GetError()))
            : this;
    }
}

/// <content> Defines the <c>WithError</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, <paramref name="sourceResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public static async Task<Result> WithError(this Task<Result> sourceResult, Func<Error, Error> onFailGetError) =>
        (await sourceResult.ConfigureAwait(false)).WithError(onFailGetError);

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, <paramref name="sourceResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> WithError<T>(this Task<Result<T>> sourceResult, Func<Error, Error> onFailGetError) =>
        (await sourceResult.ConfigureAwait(false)).WithError(onFailGetError);

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, <paramref name="sourceResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> WithError<T>(this Task<Maybe<T>> sourceResult, Func<Error, Error> onFailGetError) =>
        (await sourceResult.ConfigureAwait(false)).WithError(onFailGetError);
}
