namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Match</c> and <c>MatchAsync</c> methods.
/// </content>
public partial struct Result<T>
{
    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public TReturn Match<TReturn>(
        Func<T, TReturn> onSuccess,
        Func<Error, TReturn> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _type == ResultType.Success
            ? onSuccess(_value!)
            : onFail(Error());
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public void Match(
        Action<T> onSuccess,
        Action<Error> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (_type == ResultType.Success)
            onSuccess(_value!);
        else
            onFail(Error());
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
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
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public Task<TReturn> MatchAsync<TReturn>(
        Func<T, Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _type == ResultType.Success
            ? onSuccess(_value!)
            : onFail(Error());
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public Task MatchAsync(
        Func<T, Task> onSuccess,
        Func<Error, Task> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _type == ResultType.Success
            ? onSuccess(_value!)
            : onFail(Error());
    }
}
