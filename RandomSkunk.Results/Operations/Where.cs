namespace RandomSkunk.Results;

/// <content> Defines the <c>Where</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public Maybe<T> Where(Func<T, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (_outcome == Outcome.Success)
        {
            return predicate(_value!)
                ? AsMaybe()
                : Maybe<T>.None;
        }

        return AsMaybe();
    }

    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<T>> Where(Func<T, Task<bool>> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (_outcome == Outcome.Success)
        {
            return await predicate(_value!).ConfigureAwait(ContinueOnCapturedContext)
                ? AsMaybe()
                : Maybe<T>.None;
        }

        return AsMaybe();
    }
}

/// <content> Defines the <c>Where</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public Maybe<T> Where(Func<T, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (_outcome == Outcome.Success)
        {
            return predicate(_value!)
                ? this
                : Maybe<T>.None;
        }

        return this;
    }

    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<T>> Where(Func<T, Task<bool>> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (_outcome == Outcome.Success)
        {
            return await predicate(_value!).ConfigureAwait(ContinueOnCapturedContext)
                ? this
                : Maybe<T>.None;
        }

        return this;
    }
}

/// <content> Defines the <c>Where</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceResult"/> is <see langword="null"/> or if
    ///     <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Result<T>> sourceResult,
        Func<T, bool> predicate) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Where(predicate);

    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceResult"/> is <see langword="null"/> or if
    ///     <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<bool>> predicate) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Where(predicate).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceResult"/> is <see langword="null"/> or if
    ///     <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, bool> predicate) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Where(predicate);

    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceResult"/> is <see langword="null"/> or if
    ///     <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<bool>> predicate) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Where(predicate).ConfigureAwait(ContinueOnCapturedContext);
}
