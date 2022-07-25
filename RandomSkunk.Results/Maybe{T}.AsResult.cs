namespace RandomSkunk.Results;

/// <content> Defines the <c>AsResult</c> method. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Converts this <see cref="Maybe{T}"/> to an equivalent <see cref="Result{T}"/>: if this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned; if this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned; if this is a <c>None</c> result, then a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.ResultIsNone"/> is returned.
    /// </summary>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public Result<T> AsResult() =>
        FlatMap(value => Result<T>.Success(value));
}
