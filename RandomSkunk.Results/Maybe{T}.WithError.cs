namespace RandomSkunk.Results;

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

        return _type == MaybeType.Fail
            ? Fail(onFailGetError(Error()))
            : this;
    }
}
