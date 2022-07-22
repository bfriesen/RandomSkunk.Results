namespace RandomSkunk.Results;

/// <content> Defines the <c>Match</c> and <c>MatchAsync</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The return type of the match method.</typeparam>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public T Match<T>(
        Func<T> onSuccess,
        Func<Error, T> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _type == ResultType.Success
            ? onSuccess()
            : onFail(Error());
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The return type of the match method.</typeparam>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>A task that represents the asynchronous match operation, which wraps the result of the matching function
    ///     evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public Task<T> MatchAsync<T>(
        Func<Task<T>> onSuccess,
        Func<Error, Task<T>> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _type == ResultType.Success
            ? onSuccess()
            : onFail(Error());
    }
}
