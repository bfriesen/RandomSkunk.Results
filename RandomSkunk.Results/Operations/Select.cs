namespace RandomSkunk.Results;

/// <content> Defines the <c>Select</c> and <c>SelectAsync</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> Select<TReturn>(Func<T, TReturn> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(_value!).ToResult(),
            _ => Result<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> SelectAsync<TReturn>(Func<T, Task<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => (await onSuccessSelector(_value!).ConfigureAwait(false)).ToResult(),
            _ => Result<TReturn>.Fail(Error()),
        };
    }
}

/// <content> Defines the <c>Select</c> and <c>SelectAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> Select<TReturn>(Func<T, TReturn> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            MaybeOutcome.Success => onSuccessSelector(_value!).ToMaybe(),
            MaybeOutcome.None => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> SelectAsync<TReturn>(Func<T, Task<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            MaybeOutcome.Success => (await onSuccessSelector(_value!).ConfigureAwait(false)).ToMaybe(),
            MaybeOutcome.None => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(Error()),
        };
    }
}

/// <content> Defines the <c>Select</c> and <c>SelectAsync</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> Select<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, TReturn> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).Select(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectAsync<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> Select<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, TReturn> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).Select(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectAsync<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectAsync(onSuccessSelector).ConfigureAwait(false);
}
