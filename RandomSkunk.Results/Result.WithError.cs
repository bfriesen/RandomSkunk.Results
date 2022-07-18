namespace RandomSkunk.Results;

/// <content> Defines the <c>WithError</c> method. </content>
public partial struct Result
{
    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <param name="getError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="getError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, the current result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getError"/> is <see langword="null"/>.</exception>
    public Result WithError(Func<Error, Error> getError)
    {
        if (getError is null) throw new ArgumentNullException(nameof(getError));

        return _type == ResultType.Fail
            ? Fail(getError(Error()))
            : this;
    }
}
