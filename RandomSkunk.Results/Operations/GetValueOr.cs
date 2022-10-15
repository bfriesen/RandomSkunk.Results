namespace RandomSkunk.Results;

/// <content> Defines the <c>GetValueOr</c> and <c>GetValueOrDefault</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <param name="fallbackValue">The fallback value to return if this is not a <c>Success</c> result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, <paramref name="fallbackValue"/>.
    ///     </returns>
    public T? GetValueOr(T? fallbackValue)
    {
        return _outcome == _successOutcome ? _value! : fallbackValue;
    }

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <param name="getFallbackValue">A function that creates the fallback value to return if this is not a <c>Success</c>
    ///     result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the value returned by the
    ///     <paramref name="getFallbackValue"/> function.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public T? GetValueOr(Func<T?> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return _outcome == _successOutcome ? _value! : getFallbackValue();
    }

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the default value of type <typeparamref name="T"/> if it is a <c>Fail</c>
    /// result.
    /// </summary>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the default value of type
    ///     <typeparamref name="T"/>.</returns>
    public T? GetValueOrDefault() => GetValueOr((T?)default);
}

/// <content> Defines the <c>GetValueOr</c> and <c>GetValueOrDefault</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> or <c>None</c>
    /// result.
    /// </summary>
    /// <param name="fallbackValue">The fallback value to return if this is not a <c>Success</c> result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, <paramref name="fallbackValue"/>.
    ///     </returns>
    public T? GetValueOr(T? fallbackValue)
    {
        return _outcome == _successOutcome ? _value! : fallbackValue;
    }

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> or <c>None</c>
    /// result.
    /// </summary>
    /// <param name="getFallbackValue">A function that creates the fallback value to return if this is not a <c>Success</c>
    ///     result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the value returned by the
    ///     <paramref name="getFallbackValue"/> function.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public T? GetValueOr(Func<T?> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return _outcome == _successOutcome ? _value! : getFallbackValue();
    }

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the default value of type <typeparamref name="T"/> if it is a <c>Fail</c>
    /// or <c>None</c> result.
    /// </summary>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the default value of type
    ///     <typeparamref name="T"/>.</returns>
    public T? GetValueOrDefault() => GetValueOr((T?)default);
}

/// <content> Defines the <c>GetValueOr</c> and <c>GetValueOrDefault</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackValue">The fallback value to return if this is not a <c>Success</c> result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, <paramref name="fallbackValue"/>.
    ///     </returns>
    public static async Task<T?> GetValueOr<T>(this Task<Result<T>> sourceResult, T? fallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).GetValueOr(fallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackValue">A function that creates the fallback value to return if this is not a <c>Success</c>
    ///     result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the value returned by the
    ///     <paramref name="getFallbackValue"/> function.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<T?> GetValueOr<T>(this Task<Result<T>> sourceResult, Func<T?> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).GetValueOr(getFallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the default value of type <typeparamref name="T"/> if it is a <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the default value of type
    ///     <typeparamref name="T"/>.</returns>
    public static async Task<T?> GetValueOrDefault<T>(this Task<Result<T>> sourceResult) =>
        (await sourceResult.ConfigureAwait(false)).GetValueOrDefault();

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> or <c>None</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackValue">The fallback value to return if this is not a <c>Success</c> result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, <paramref name="fallbackValue"/>.
    ///     </returns>
    public static async Task<T?> GetValueOr<T>(this Task<Maybe<T>> sourceResult, T? fallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).GetValueOr(fallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> or <c>None</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackValue">A function that creates the fallback value to return if this is not a <c>Success</c>
    ///     result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the value returned by the
    ///     <paramref name="getFallbackValue"/> function.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<T?> GetValueOr<T>(this Task<Maybe<T>> sourceResult, Func<T?> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).GetValueOr(getFallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the default value of type <typeparamref name="T"/> if it is a <c>Fail</c>
    /// or <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the default value of type
    ///     <typeparamref name="T"/>.</returns>
    public static async Task<T?> GetValueOrDefault<T>(this Task<Maybe<T>> sourceResult) =>
        (await sourceResult.ConfigureAwait(false)).GetValueOrDefault();
}
