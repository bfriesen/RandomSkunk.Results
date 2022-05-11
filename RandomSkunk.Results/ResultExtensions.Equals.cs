using static RandomSkunk.Results.MaybeType;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Equals</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="otherValue">The value to compare.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> used to determine equality of the values.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Success</c> result and its value equals
    /// <paramref name="otherValue"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(this Result<T> source, T otherValue, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        return source._type == Success && comparer.Equals(source._value!, otherValue);
    }

    /// <summary>
    /// Determines whether the value of the result is equal to another value as defined by the
    /// <paramref name="isSuccessValue"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="isSuccessValue">
    /// A function that defines the equality of the result value.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Success</c> result and
    /// <paramref name="isSuccessValue"/> evaluates to <see langword="true"/> when passed
    /// its value; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="isSuccessValue"/> is <see langword="null"/>.
    /// </exception>
    public static bool Equals<T>(this Result<T> source, Func<T, bool> isSuccessValue)
    {
        if (isSuccessValue is null) throw new ArgumentNullException(nameof(isSuccessValue));

        return source._type == Success && isSuccessValue(source._value!);
    }

    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="otherValue">The value to compare.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> used to determine equality of the values. If
    /// <see langword="null"/>, then <see cref="EqualityComparer{T}.Default"/> is used instead.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Some</c> result and its value equals
    /// <paramref name="otherValue"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(this Maybe<T> source, T otherValue, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        return source._type == Some && comparer.Equals(source._value!, otherValue);
    }

    /// <summary>
    /// Determines whether the value of the result is equal to another value as defined by the
    /// <paramref name="isSomeValue"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="isSomeValue">
    /// A function that defines the equality of the result value.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Some</c> result and
    /// <paramref name="isSomeValue"/> evaluates to <see langword="true"/> when passed
    /// its value; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="isSomeValue"/> is <see langword="null"/>.
    /// </exception>
    public static bool Equals<T>(this Maybe<T> source, Func<T, bool> isSomeValue)
    {
        if (isSomeValue is null) throw new ArgumentNullException(nameof(isSomeValue));

        return source._type == Some && isSomeValue(source._value!);
    }
}
