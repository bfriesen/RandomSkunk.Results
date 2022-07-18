namespace RandomSkunk.Results;

/// <content> Defines the <c>AsMaybe</c> method. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Converts this <see cref="Result{T}"/> to an equivalent <see cref="Maybe{T}"/>: if this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned; if this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned; a <c>None</c> result is never returned.
    /// </summary>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public Maybe<T> AsMaybe(Func<Error, Error>? onFail = null) =>
        Then(value => value.ToMaybe(), onFail);
}
