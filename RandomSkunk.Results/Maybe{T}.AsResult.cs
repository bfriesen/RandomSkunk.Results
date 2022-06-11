namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>AsResult</c> method.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Converts this <see cref="Maybe{T}"/> to an equivalent <see cref="Result{T}"/>: if this is a <c>Some</c> result, then a
    /// <c>Success</c> result with the same value is returned; if this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned; if this is a <c>None</c> result, then a <c>Fail</c> result with a "Not Found" error (error
    /// code: 404) is returned.
    /// </summary>
    /// <param name="onNone">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultGetNoneError"/> is used
    /// instead. Evaluated only if this is a <c>None</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if this is a <c>Fail</c> result.
    /// </param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public Result<T> AsResult(Func<Error>? onNone = null, Func<Error, Error>? onFail = null) =>
        CrossMap(value => value.ToResult(), onNone, onFail);
}
