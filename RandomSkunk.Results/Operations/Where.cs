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

        if (IsSuccess)
        {
            return predicate(_value!)
                ? AsMaybe()
                : Maybe<T>.None();
        }

        return AsMaybe();
    }

    /// <inheritdoc cref="Where(Func{T, bool})"/>
    public async Task<Maybe<T>> Where(Func<T, Task<bool>> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (IsSuccess)
        {
            return await predicate(_value!).ConfigureAwait(false)
                ? AsMaybe()
                : Maybe<T>.None();
        }

        return AsMaybe();
    }
}

/// <content> Defines the <c>Where</c> methods. </content>
public partial struct Maybe<T>
{
    /// <inheritdoc cref="Result{T}.Where(Func{T, bool})"/>
    public Maybe<T> Where(Func<T, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (IsSuccess)
        {
            return predicate(_value!)
                ? this
                : Maybe<T>.None();
        }

        return this;
    }

    /// <inheritdoc cref="Result{T}.Where(Func{T, bool})"/>
    public async Task<Maybe<T>> Where(Func<T, Task<bool>> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (IsSuccess)
        {
            return await predicate(_value!).ConfigureAwait(false)
                ? this
                : Maybe<T>.None();
        }

        return this;
    }
}

/// <content> Defines the <c>Where</c> extension methods. </content>
public static partial class ResultExtensions
{
    #pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

    /// <inheritdoc cref="Result{T}.Where(Func{T, bool})"/>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Result<T>> sourceResult,
        Func<T, bool> predicate) =>
        (await sourceResult.ConfigureAwait(false)).Where(predicate);

    /// <inheritdoc cref="Where{T}(Task{Result{T}},Func{T, bool})"/>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<bool>> predicate) =>
        await (await sourceResult.ConfigureAwait(false)).Where(predicate).ConfigureAwait(false);

    /// <inheritdoc cref="Where{T}(Task{Result{T}},Func{T, bool})"/>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, bool> predicate) =>
        (await sourceResult.ConfigureAwait(false)).Where(predicate);

    /// <inheritdoc cref="Where{T}(Task{Result{T}},Func{T, bool})"/>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<bool>> predicate) =>
        await (await sourceResult.ConfigureAwait(false)).Where(predicate).ConfigureAwait(false);

    #pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
}
