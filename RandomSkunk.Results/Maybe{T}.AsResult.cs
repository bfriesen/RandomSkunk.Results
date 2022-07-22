namespace RandomSkunk.Results;

/// <content> Defines the <c>AsResult</c> method. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Converts this <see cref="Maybe{T}"/> to an equivalent <see cref="Result{T}"/>: if this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned; if this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned; if this is a <c>None</c> result, then a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NotFound"/>) is returned.
    /// </summary>
    /// <param name="onNone">An optional function that maps a <c>None</c> result to the returned <c>Fail</c> result's error.
    ///     <list type="bullet">
    ///         <item>
    ///             This function is evaluated <em>only</em> if this is a <c>None</c> result.
    ///         </item>
    ///         <item>
    ///             When <see langword="null"/> or not provided, an error with error code <see cref="ErrorCodes.NotFound"/> and
    ///             message similar to "Not Found" will be used instead.
    ///         </item>
    ///         <item>
    ///             When provided, the function should not return <see langword="null"/>. If it does, an error with error code
    ///             <see cref="ErrorCodes.BadRequest"/> and a message similar to "Function parameter should not return null" will
    ///             be used instead.
    ///         </item>
    ///     </list>
    /// </param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned <c>Fail</c> result's
    ///     error.
    ///     <list type="bullet">
    ///         <item>
    ///             This function is evaluated <em>only</em> if this is a <c>Fail</c> result.
    ///         </item>
    ///         <item>
    ///             If <see langword="null"/> or not provided, no transformation takes place - this <c>Fail</c> result's error is
    ///             used as the error of the returned <c>Fail</c> result.
    ///         </item>
    ///     </list>
    /// </param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public Result<T> AsResult(Func<Error>? onNone = null, Func<Error, Error>? onFail = null) =>
        FlatMap(value => Result<T>.Success(value), onNone, onFail);
}
