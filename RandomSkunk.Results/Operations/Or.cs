namespace RandomSkunk.Results;

/// <content> Defines the <c>Or</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Returns the current result if it is a <c>Success</c> result; otherwise, returns a new <c>Success</c> result with the
    /// specified fallback value.
    /// </summary>
    /// <param name="fallbackValue">The fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="fallbackValue"/> is <see langword="null"/>.</exception>
    public Result<T> Or([DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return _outcome == _successOutcome ? this : fallbackValue.ToResult();
    }

    /// <summary>
    /// Returns the current result if it is a <c>Success</c> result; otherwise, returns a new <c>Success</c> result with the
    /// specified fallback value.
    /// </summary>
    /// <param name="getFallbackValue">A function that returns the fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public Result<T> Or(Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return _outcome == _successOutcome ? this : getFallbackValue().ToResult();
    }
}

/// <content> Defines the <c>Or</c> methods. </content>
public partial struct Maybe<T>
{
    /// <inheritdoc cref="Result{T}.Or(T)"/>
    public Maybe<T> Or([DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return _outcome == _successOutcome ? this : fallbackValue.ToMaybe();
    }

    /// <inheritdoc cref="Result{T}.Or(Func{T})"/>
    public Maybe<T> Or(Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return _outcome == _successOutcome ? this : getFallbackValue().ToMaybe();
    }
}

/// <content> Defines the <c>Or</c> extension methods. </content>
public static partial class ResultExtensions
{
    #pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

    /// <inheritdoc cref="Result{T}.Or(T)"/>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    public static async Task<Result<T>> Or<T>(this Task<Result<T>> sourceResult, [DisallowNull] T fallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).Or(fallbackValue);

    /// <inheritdoc cref="Result{T}.Or(Func{T})"/>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    public static async Task<Result<T>> Or<T>(this Task<Result<T>> sourceResult, Func<T> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).Or(getFallbackValue);

    /// <inheritdoc cref="Or{T}(Task{Result{T}}, T)"/>
    public static async Task<Maybe<T>> Or<T>(this Task<Maybe<T>> sourceResult, [DisallowNull] T fallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).Or(fallbackValue);

    /// <inheritdoc cref="Or{T}(Task{Result{T}}, Func{T})"/>
    public static async Task<Maybe<T>> Or<T>(this Task<Maybe<T>> sourceResult, Func<T> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).Or(getFallbackValue);

    #pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
}
