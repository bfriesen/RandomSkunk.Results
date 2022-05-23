namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Match</c> and <c>MatchAsync</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<T> Match<T>(
        this Task<Result> source,
        Func<T> success,
        Func<Error, T> fail) =>
        (await source).Match(success, fail);

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task that represents the match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task Match(
        this Task<Result> source,
        Action success,
        Action<Error> fail) =>
        (await source).Match(success, fail);

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<T> MatchAsync<T>(
        this Task<Result> source,
        Func<Task<T>> success,
        Func<Error, Task<T>> fail) =>
        await (await source).MatchAsync(success, fail);

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task MatchAsync(
        this Task<Result> source,
        Func<Task> success,
        Func<Error, Task> fail) =>
        await (await source).MatchAsync(success, fail);

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, TReturn> success,
        Func<Error, TReturn> fail) =>
            (await source).Match(success, fail);

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task that represents the match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task Match<T>(
        this Task<Result<T>> source,
        Action<T> success,
        Action<Error> fail) =>
            (await source).Match(success, fail);

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<TReturn> MatchAsync<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, Task<TReturn>> success,
        Func<Error, Task<TReturn>> fail) =>
            await (await source).MatchAsync(success, fail);

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task MatchAsync<T>(
        this Task<Result<T>> source,
        Func<T, Task> success,
        Func<Error, Task> fail) =>
            await (await source).MatchAsync(success, fail);

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, TReturn> some,
        Func<TReturn> none,
        Func<Error, TReturn> fail) =>
        (await source).Match(some, none, fail);

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task that represents the match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public static async Task Match<T>(
        this Task<Maybe<T>> source,
        Action<T> some,
        Action none,
        Action<Error> fail) =>
        (await source).Match(some, none, fail);

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<TReturn> MatchAsync<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, Task<TReturn>> some,
        Func<Task<TReturn>> none,
        Func<Error, Task<TReturn>> fail) =>
        await (await source).MatchAsync(some, none, fail);

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public static async Task MatchAsync<T>(
        this Task<Maybe<T>> source,
        Func<T, Task> some,
        Func<Task> none,
        Func<Error, Task> fail) =>
        await (await source).MatchAsync(some, none, fail);
}
