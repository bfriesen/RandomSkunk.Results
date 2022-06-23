namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Match</c> and <c>MatchAsync</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>Success</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/>, or if <paramref name="onNone"/> is
    /// <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    /// </exception>
    public TReturn Match<TReturn>(
        Func<T, TReturn> onSuccess,
        Func<TReturn> onNone,
        Func<Error, TReturn> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onNone is null) throw new ArgumentNullException(nameof(onNone));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _type switch
        {
            MaybeType.Success => onSuccess(_value!),
            MaybeType.None => onNone(),
            _ => onFail(Error()),
        };
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>Success</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/>, or if <paramref name="onNone"/> is
    /// <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    /// </exception>
    public void Match(
        Action<T> onSuccess,
        Action onNone,
        Action<Error> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onNone is null) throw new ArgumentNullException(nameof(onNone));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        switch (_type)
        {
            case MaybeType.Success: onSuccess(_value!); break;
            case MaybeType.None: onNone(); break;
            default: onFail(Error()); break;
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>Success</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/>, or if <paramref name="onNone"/> is
    /// <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    /// </exception>
    public Task<TReturn> MatchAsync<TReturn>(
        Func<T, Task<TReturn>> onSuccess,
        Func<Task<TReturn>> onNone,
        Func<Error, Task<TReturn>> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onNone is null) throw new ArgumentNullException(nameof(onNone));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _type switch
        {
            MaybeType.Success => onSuccess(_value!),
            MaybeType.None => onNone(),
            _ => onFail(Error()),
        };
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>Success</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/>, or if <paramref name="onNone"/> is
    /// <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    /// </exception>
    public Task MatchAsync(
        Func<T, Task> onSuccess,
        Func<Task> onNone,
        Func<Error, Task> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onNone is null) throw new ArgumentNullException(nameof(onNone));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _type switch
        {
            MaybeType.Success => onSuccess(_value!),
            MaybeType.None => onNone(),
            _ => onFail(Error()),
        };
    }
}
