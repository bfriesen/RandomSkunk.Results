namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>WithError</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="getError">
    /// A function that returns the error for the returned <c>Fail</c> result.
    /// </param>
    /// <returns>
    /// A new <c>Fail</c> result with its error specified by the <paramref name="getError"/>
    /// function if this is a <c>Fail</c> result; otherwise, <paramref name="source"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getError"/> is <see langword="null"/>.
    /// </exception>
    public static Result WithError(this Result source, Func<Error, Error> getError)
    {
        if (getError is null) throw new ArgumentNullException(nameof(getError));

        return source._type == ResultType.Fail
            ? Result.Create.Fail(getError(source.Error()))
            : source;
    }

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getError">
    /// A function that returns the error for the returned <c>Fail</c> result.
    /// </param>
    /// <returns>
    /// A new <c>Fail</c> result with its error specified by the <paramref name="getError"/>
    /// function if this is a <c>Fail</c> result; otherwise, <paramref name="source"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getError"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> WithError<T>(this Result<T> source, Func<Error, Error> getError)
    {
        if (getError is null) throw new ArgumentNullException(nameof(getError));

        return source._type == ResultType.Fail
            ? Result<T>.Create.Fail(getError(source.Error()))
            : source;
    }

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getError">
    /// A function that returns the error for the returned <c>Fail</c> result.
    /// </param>
    /// <returns>
    /// A new <c>Fail</c> result with its error specified by the <paramref name="getError"/>
    /// function if this is a <c>Fail</c> result; otherwise, <paramref name="source"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getError"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> WithError<T>(this Maybe<T> source, Func<Error, Error> getError)
    {
        if (getError is null) throw new ArgumentNullException(nameof(getError));

        return source._type == MaybeType.Fail
            ? Maybe<T>.Create.Fail(getError(source.Error()))
            : source;
    }
}
