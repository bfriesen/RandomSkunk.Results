namespace RandomSkunk.Results;

/// <content> Defines the <c>AsMaybe</c> method. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Converts this <see cref="Result{T}"/> to an equivalent <see cref="Maybe{T}"/>. If this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned. Otherwise, if the <c>Fail</c> result's error has error code
    /// <see cref="ErrorCodes.ResultIsNone"/>, then a <c>None</c> result is returned. For any other error code, a new <c>Fail</c>
    /// result with the same error is returned.
    /// </summary>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public Maybe<T> AsMaybe() =>
        FlatMap(value => Maybe<T>.Success(value));
}
