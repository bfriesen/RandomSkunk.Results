namespace RandomSkunk.Results.Unsafe;

/// <summary>
/// Defines extension methods for result objects that throw an exception if not in the expected
/// state.
/// </summary>
public static class UnsafeResultExtensions
{
    /// <summary>
    /// Gets the error from the <c>Fail</c> result.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <returns>
    /// If <paramref name="source"/> is a <c>Fail</c> result, its error; otherwise throws an
    /// <see cref="InvalidStateException"/>.
    /// </returns>
    /// <exception cref="InvalidStateException">
    /// If the result is not a <c>Fail</c> result.
    /// </exception>
    public static Error GetError(this Result source) =>
        source.IsFail
            ? source.Error
            : throw Exceptions.CannotAccessErrorUnlessFail;

    /// <summary>
    /// Gets the value from the <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>
    /// If <paramref name="source"/> is a <c>Success</c> result, its value; otherwise throws an
    /// <see cref="InvalidStateException"/>.
    /// </returns>
    /// <exception cref="InvalidStateException">
    /// If the result is not a <c>Success</c> result.
    /// </exception>
    public static T GetValue<T>(this Result<T> source) =>
        source.IsSuccess
            ? source.Value
            : throw Exceptions.CannotAccessValueUnlessSuccess;

    /// <summary>
    /// Gets the error from the <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>
    /// If <paramref name="source"/> is a <c>Fail</c> result, its error; otherwise throws an
    /// <see cref="InvalidStateException"/>.
    /// </returns>
    /// <exception cref="InvalidStateException">
    /// If the result is not a <c>Fail</c> result.
    /// </exception>
    public static Error GetError<T>(this Result<T> source) =>
        source.IsFail
            ? source.Error
            : throw Exceptions.CannotAccessErrorUnlessFail;

    /// <summary>
    /// Gets the value from the <c>Some</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>
    /// If <paramref name="source"/> is a <c>Some</c> result, its value; otherwise throws an
    /// <see cref="InvalidStateException"/>.
    /// </returns>
    /// <exception cref="InvalidStateException">
    /// If the result is not a <c>Some</c> result.
    /// </exception>
    public static T GetValue<T>(this Maybe<T> source) =>
        source.IsSome
            ? source.Value
            : throw Exceptions.CannotAccessValueUnlessSome;

    /// <summary>
    /// Gets the error from the <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>
    /// If <paramref name="source"/> is a <c>Fail</c> result, its error; otherwise throws an
    /// <see cref="InvalidStateException"/>.
    /// </returns>
    /// <exception cref="InvalidStateException">
    /// If the result is not a <c>Fail</c> result.
    /// </exception>
    public static Error GetError<T>(this Maybe<T> source) =>
        source.IsFail
            ? source.Error
            : throw Exceptions.CannotAccessErrorUnlessFail;
}
