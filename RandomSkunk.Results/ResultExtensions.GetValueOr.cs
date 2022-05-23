using static RandomSkunk.Results.MaybeType;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>GetValueOr</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">
    /// The fallback value to return if this is not a <c>Success</c> result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Success</c> result; otherwise,
    /// <paramref name="fallbackValue"/>.
    /// </returns>
    public static T GetValueOr<T>(this Result<T> source, T fallbackValue)
    {
        return source._type == Success ? source._value! : fallbackValue;
    }

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that creates the fallback value to return if this is not a <c>Success</c>
    /// result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Success</c> result; otherwise, the value returned
    /// by the <paramref name="getFallbackValue"/> function.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static T GetValueOr<T>(this Result<T> source, Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return source._type == Success ? source._value! : getFallbackValue();
    }

    /// <summary>
    /// Gets the value of the <c>Some</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">
    /// The fallback value to return if this is not a <c>Some</c> result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Some</c> result; otherwise,
    /// <paramref name="fallbackValue"/>.
    /// </returns>
    public static T GetValueOr<T>(this Maybe<T> source, T fallbackValue)
    {
        return source._type == Some ? source._value! : fallbackValue;
    }

    /// <summary>
    /// Gets the value of the <c>Some</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that creates the fallback value to return if this is not a <c>Some</c>
    /// result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Some</c> result; otherwise, the value returned
    /// by the <paramref name="getFallbackValue"/> function.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static T GetValueOr<T>(this Maybe<T> source, Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return source._type == Some ? source._value! : getFallbackValue();
    }

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">
    /// The fallback value to return if this is not a <c>Success</c> result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Success</c> result; otherwise,
    /// <paramref name="fallbackValue"/>.
    /// </returns>
    public static async Task<T> GetValueOr<T>(this Task<Result<T>> source, T fallbackValue) =>
        (await source).GetValueOr(fallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that creates the fallback value to return if this is not a <c>Success</c>
    /// result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Success</c> result; otherwise, the value returned
    /// by the <paramref name="getFallbackValue"/> function.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<T> GetValueOr<T>(this Task<Result<T>> source, Func<T> getFallbackValue) =>
        (await source).GetValueOr(getFallbackValue);

    /// <summary>
    /// Gets the value of the <c>Some</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">
    /// The fallback value to return if this is not a <c>Some</c> result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Some</c> result; otherwise,
    /// <paramref name="fallbackValue"/>.
    /// </returns>
    public static async Task<T> GetValueOr<T>(this Task<Maybe<T>> source, T fallbackValue) =>
        (await source).GetValueOr(fallbackValue);

    /// <summary>
    /// Gets the value of the <c>Some</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that creates the fallback value to return if this is not a <c>Some</c>
    /// result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Some</c> result; otherwise, the value returned
    /// by the <paramref name="getFallbackValue"/> function.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<T> GetValueOr<T>(this Task<Maybe<T>> source, Func<T> getFallbackValue) =>
        (await source).GetValueOr(getFallbackValue);
}
