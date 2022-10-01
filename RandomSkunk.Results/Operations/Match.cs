namespace RandomSkunk.Results;

/// <content> Defines the <c>Match</c> and <c>MatchAsync</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public TReturn Match<TReturn>(
        Func<TReturn> onSuccess,
        Func<Error, TReturn> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _outcome == _successOutcome
            ? onSuccess()
            : onFail(GetError());
    }

    /// <inheritdoc cref="Match{TReturn}(Func{TReturn}, Func{Error, TReturn})"/>
    public Task<TReturn> MatchAsync<TReturn>(
        Func<Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _outcome == _successOutcome
            ? onSuccess()
            : onFail(GetError());
    }
}

/// <content> Defines the <c>Match</c> and <c>MatchAsync</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public TReturn Match<TReturn>(
        Func<T, TReturn> onSuccess,
        Func<Error, TReturn> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _outcome == _successOutcome
            ? onSuccess(_value!)
            : onFail(GetError());
    }

    /// <inheritdoc cref="Match{TReturn}(Func{T, TReturn}, Func{Error, TReturn})"/>
    public Task<TReturn> MatchAsync<TReturn>(
        Func<T, Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _outcome == _successOutcome
            ? onSuccess(_value!)
            : onFail(GetError());
    }
}

/// <content> Defines the <c>Match</c> and <c>MatchAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/>, <paramref name="onNone"/>, or <paramref name="onFail"/> function
    /// depending on whether the result is <c>Success</c>, <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onNone">The function to evaluate if the result is <c>None</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>, or if
    ///     <paramref name="onNone"/> is <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    ///     </exception>
    public TReturn Match<TReturn>(
        Func<T, TReturn> onSuccess,
        Func<TReturn> onNone,
        Func<Error, TReturn> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onNone is null) throw new ArgumentNullException(nameof(onNone));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _outcome switch
        {
            _successOutcome => onSuccess(_value!),
            _noneOutcome => onNone(),
            _ => onFail(GetError()),
        };
    }

    /// <inheritdoc cref="Match{TReturn}(Func{T, TReturn}, Func{TReturn}, Func{Error, TReturn})"/>
    public Task<TReturn> MatchAsync<TReturn>(
        Func<T, Task<TReturn>> onSuccess,
        Func<Task<TReturn>> onNone,
        Func<Error, Task<TReturn>> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onNone is null) throw new ArgumentNullException(nameof(onNone));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _outcome switch
        {
            _successOutcome => onSuccess(_value!),
            _noneOutcome => onNone(),
            _ => onFail(GetError()),
        };
    }
}

/// <content> Defines the <c>Match</c> and <c>MatchAsync</c> extension methods. </content>
public static partial class ResultExtensions
{
    #pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
    #pragma warning disable CS1712 // Type parameter has no matching typeparam tag in the XML comment (but other type parameters do)

    /// <inheritdoc cref="Result.Match{TReturn}(Func{TReturn}, Func{Error, TReturn})"/>
    /// <typeparam name="TReturn">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    public static async Task<TReturn> Match<TReturn>(
        this Task<Result> sourceResult,
        Func<TReturn> onSuccess,
        Func<Error, TReturn> onFail) =>
        (await sourceResult.ConfigureAwait(false)).Match(onSuccess, onFail);

    /// <inheritdoc cref="Match{T}(Task{Result}, Func{T}, Func{Error, T})"/>
    public static async Task<TReturn> MatchAsync<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFail) =>
        await (await sourceResult.ConfigureAwait(false)).MatchAsync(onSuccess, onFail).ConfigureAwait(false);

    /// <inheritdoc cref="Result{T}.Match{TReturn}(Func{T, TReturn}, Func{Error, TReturn})"/>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, TReturn> onSuccess,
        Func<Error, TReturn> onFail) =>
            (await sourceResult.ConfigureAwait(false)).Match(onSuccess, onFail);

    /// <inheritdoc cref="Match{T, TReturn}(Task{Result{T}}, Func{T, TReturn}, Func{Error, TReturn})"/>
    public static async Task<TReturn> MatchAsync<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFail) =>
            await (await sourceResult.ConfigureAwait(false)).MatchAsync(onSuccess, onFail).ConfigureAwait(false);

    /// <inheritdoc cref="Maybe{T}.Match{TReturn}(Func{T, TReturn}, Func{TReturn}, Func{Error, TReturn})"/>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, TReturn> onSuccess,
        Func<TReturn> onNone,
        Func<Error, TReturn> onFail) =>
        (await sourceResult.ConfigureAwait(false)).Match(onSuccess, onNone, onFail);

    /// <inheritdoc cref="Match{T, TReturn}(Task{Maybe{T}}, Func{T, TReturn}, Func{TReturn}, Func{Error, TReturn})"/>
    public static async Task<TReturn> MatchAsync<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccess,
        Func<Task<TReturn>> onNone,
        Func<Error, Task<TReturn>> onFail) =>
        await (await sourceResult.ConfigureAwait(false)).MatchAsync(onSuccess, onNone, onFail).ConfigureAwait(false);

    #pragma warning restore CS1712 // Type parameter has no matching typeparam tag in the XML comment (but other type parameters do)
    #pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
}
