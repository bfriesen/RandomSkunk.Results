namespace RandomSkunk.Results;

/// <content> Defines the <c>FlatMap</c> and <c>FlatMapAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this
    /// is a <c>None</c> result, return a <c>None</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> FlatMap<TReturn>(Func<T, Maybe<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => onSuccessSelector(_value!),
            MaybeType.None => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this
    /// is a <c>None</c> result, return a <c>None</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> FlatMapAsync<TReturn>(Func<T, Task<Maybe<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            MaybeType.None => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this
    /// is a <c>None</c> result, return a <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <param name="onNoneGetError">An optional function that maps a <c>None</c> result to the returned <c>Fail</c> result's
    ///     error.
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
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result FlatMap(Func<T, Result> onSuccessSelector, Func<Error>? onNoneGetError = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => onSuccessSelector(_value!),
            MaybeType.None => Result.Fail(onNoneGetError?.Invoke() ?? Errors.NotFound()),
            _ => Result.Fail(Error()),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this
    /// is a <c>None</c> result, return a <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <param name="onNoneGetError">An optional function that maps a <c>None</c> result to the returned <c>Fail</c> result's
    ///     error.
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
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result> FlatMapAsync(Func<T, Task<Result>> onSuccessSelector, Func<Error>? onNoneGetError = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            MaybeType.None => Result.Fail(onNoneGetError?.Invoke() ?? Errors.NotFound()),
            _ => Result.Fail(Error()),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this
    /// is a <c>None</c> result, return a <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <param name="onNoneGetError">An optional function that maps a <c>None</c> result to the returned <c>Fail</c> result's
    ///     error.
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
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> FlatMap<TReturn>(Func<T, Result<TReturn>> onSuccessSelector, Func<Error>? onNoneGetError = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => onSuccessSelector(_value!),
            MaybeType.None => Result<TReturn>.Fail(onNoneGetError?.Invoke() ?? Errors.NotFound()),
            _ => Result<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this
    /// is a <c>None</c> result, return a <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <param name="onNoneGetError">An optional function that maps a <c>None</c> result to the returned <c>Fail</c> result's
    ///     error.
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
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> FlatMapAsync<TReturn>(
        Func<T, Task<Result<TReturn>>> onSuccessSelector,
        Func<Error>? onNoneGetError = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            MaybeType.None => Result<TReturn>.Fail(onNoneGetError?.Invoke() ?? Errors.NotFound()),
            _ => Result<TReturn>.Fail(Error()),
        };
    }
}
