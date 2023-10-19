namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Gets an object that, when disposed, disposes the source result's value.
    /// </summary>
    /// <typeparam name="TDisposable">The <see cref="IDisposable"/> type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An <see cref="IDisposable"/> object.</returns>
    public static IDisposable AsDisposable<TDisposable>(this Result<TDisposable> sourceResult)
        where TDisposable : IDisposable =>
        new DisposableResult<TDisposable>(sourceResult);

    /// <summary>
    /// Gets an object that, when disposed, disposes the source result's value.
    /// </summary>
    /// <typeparam name="TAsyncDisposable">The <see cref="IAsyncDisposable"/> type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> object.</returns>
    public static IAsyncDisposable AsAsyncDisposable<TAsyncDisposable>(this Result<TAsyncDisposable> sourceResult)
        where TAsyncDisposable : IAsyncDisposable =>
        new AsyncDisposableResult<TAsyncDisposable>(sourceResult);

    /// <summary>
    /// Gets an equivalent result with a non-nullable type.
    /// </summary>
    /// <typeparam name="T">The nullable type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <returns>The equivalant result.</returns>
    public static Result<T> AsNonNullable<T>(this Result<T?> result)
        where T : struct =>
        result.Select(value => value!.Value);

    /// <summary>
    /// Gets an equivalent result with a non-nullable type.
    /// </summary>
    /// <typeparam name="T">The nullable type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <returns>The equivalant result.</returns>
    public static async Task<Result<T>> AsNonNullable<T>(this Task<Result<T?>> result)
        where T : struct =>
        (await result.ConfigureAwait(ContinueOnCapturedContext)).AsNonNullable();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result<T> Flatten<T>(this Result<Result<T>> sourceResult) =>
        sourceResult.SelectMany(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result Flatten(this Result<Result> sourceResult) =>
        sourceResult.SelectMany(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Result<T>> Flatten<T>(this Task<Result<Result<T>>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Flatten();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Result> Flatten(this Task<Result<Result>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Flatten();

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackValue">The fallback value to return if this is not a <c>Success</c> result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, <paramref name="fallbackValue"/>.
    ///     </returns>
    public static async Task<T?> GetValueOr<T>(this Task<Result<T>> sourceResult, T? fallbackValue) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).GetValueOr(fallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackValue">A function that creates the fallback value to return if this is not a <c>Success</c>
    ///     result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the value returned by the
    ///     <paramref name="getFallbackValue"/> function.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<T?> GetValueOr<T>(this Task<Result<T>> sourceResult, Func<T?> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).GetValueOr(getFallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the default value of type <typeparamref name="T"/> if it is a <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the default value of type
    ///     <typeparamref name="T"/>.</returns>
    public static async Task<T?> GetValueOrDefault<T>(this Task<Result<T>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).GetValueOrDefault();

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public static async Task<TReturn> Match<TReturn>(
        this Task<Result> sourceResult,
        Func<TReturn> onSuccess,
        Func<Error, TReturn> onFail) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Match(onSuccess, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>A task that represents the asynchronous match operation, which wraps the result of the matching function
    ///     evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public static async Task<TReturn> Match<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFail) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Match(onSuccess, onFail).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, TReturn> onSuccess,
        Func<Error, TReturn> onFail) =>
            (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Match(onSuccess, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>A task that represents the asynchronous match operation, which wraps the result of the matching function
    ///     evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFail) =>
            await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Match(onSuccess, onFail).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/>, <paramref name="onNone"/>, or <paramref name="onFail"/> function
    /// depending on whether the result is <c>Success</c>, <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onNone">The function to evaluate if the result is <c>None</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>, or if
    ///     <paramref name="onNone"/> is <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    ///     </exception>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, TReturn> onSuccess,
        Func<TReturn> onNone,
        Func<Error, TReturn> onFail) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Match(onSuccess, onNone, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/>, <paramref name="onNone"/>, or <paramref name="onFail"/> function
    /// depending on whether the result is <c>Success</c>, <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onNone">The function to evaluate if the result is <c>None</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>A task that represents the asynchronous match operation, which wraps the result of the matching function
    ///     evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>, or if
    ///     <paramref name="onNone"/> is <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    ///     </exception>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccess,
        Func<Task<TReturn>> onNone,
        Func<Error, Task<TReturn>> onFail) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Match(onSuccess, onNone, onFail).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result> OnFail(this Task<Result> sourceResult, Action<Error> onFailCallback) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnFail(onFailCallback);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result> OnFail(this Task<Result> sourceResult, Func<Error, Task> onFailCallback) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnFail(onFailCallback).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnFail<T>(this Task<Result<T>> sourceResult, Action<Error> onFailCallback) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnFail(onFailCallback);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnFail<T>(this Task<Result<T>> sourceResult, Func<Error, Task> onFailCallback) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnFail(onFailCallback).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public static TResult OnFail<TResult>(
        this TResult sourceResult,
        Action<Error> onFail)
        where TResult : IResult
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (!sourceResult.IsSuccess)
        {
            var error = sourceResult.Error;
            onFail(error);
        }

        return sourceResult;
    }

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnFail<TResult>(
        this TResult sourceResult,
        Func<Error, Task> onFail)
        where TResult : IResult
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (!sourceResult.IsSuccess)
        {
            var error = sourceResult.Error;
            await onFail(error).ConfigureAwait(ContinueOnCapturedContext);
        }

        return sourceResult;
    }

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnFail<TResult>(
        this Task<TResult> sourceResult,
        Action<Error> onFail)
        where TResult : IResult =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnFail(onFail);

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnFail<TResult>(
        this Task<TResult> sourceResult,
        Func<Error, Task> onFail)
        where TResult : IResult =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnFail(onFail).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Invokes the <paramref name="onNoneCallback"/> function if <paramref name="sourceResult"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneCallback">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnNone<T>(this Task<Result<T>> sourceResult, Action onNoneCallback) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnNone(onNoneCallback);

    /// <summary>
    /// Invokes the <paramref name="onNoneCallback"/> function if <paramref name="sourceResult"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneCallback">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnNone<T>(this Task<Result<T>> sourceResult, Func<Task> onNoneCallback) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnNone(onNoneCallback).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result> OnSuccess(this Task<Result> sourceResult, Action onSuccessCallback) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnSuccess(onSuccessCallback);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result> OnSuccess(this Task<Result> sourceResult, Func<Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnSuccess(onSuccessCallback).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> sourceResult, Action<T> onSuccessCallback) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnSuccess(onSuccessCallback);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> sourceResult, Func<T, Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnSuccess(onSuccessCallback).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public static async Task<Result> Rescue(this Task<Result> sourceResult, Func<Error, Result> onFail) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Rescue(onFail);

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public static async Task<Result> Rescue(this Task<Result> sourceResult, Func<Error, Task<Result>> onFail) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Rescue(onFail).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public static async Task<Result<T>> Rescue<T>(this Task<Result<T>> sourceResult, Func<Error, Result<T>> onFail) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Rescue(onFail);

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public static async Task<Result<T>> Rescue<T>(this Task<Result<T>> sourceResult, Func<Error, Task<Result<T>>> onFail) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Rescue(onFail).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned. Rescues a <c>None</c> result with the
    /// <paramref name="onNone"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <param name="onNone">An optional function that rescues a <c>None</c> result.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public static async Task<Result<T>> Rescue<T>(
        this Task<Result<T>> sourceResult,
        Func<Error, Result<T>> onFail,
        Func<Result<T>> onNone) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Rescue(onFail, onNone);

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned. Rescues a <c>None</c> result with the
    /// <paramref name="onNone"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <param name="onNone">An optional function that rescues a <c>None</c> result.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public static async Task<Result<T>> Rescue<T>(
        this Task<Result<T>> sourceResult,
        Func<Error, Task<Result<T>>> onFail,
        Func<Task<Result<T>>> onNone) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Rescue(onFail, onNone).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form as a
    /// <c>Success</c> result by passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is
    /// projected to the new form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the value returned by <paramref name="onSuccessSelector"/>.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> Select<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, TReturn> onSuccessSelector,
        Func<TReturn>? onNoneSelector = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Select(onSuccessSelector, onNoneSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form as a
    /// <c>Success</c> result by passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is
    /// projected to the new form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the value returned by <paramref name="onSuccessSelector"/>.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> Select<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccessSelector,
        Func<Task<TReturn>>? onNoneSelector = null) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Select(onSuccessSelector, onNoneSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form as a
    /// <c>Success</c> result by passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is
    /// projected to the new form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the value returned by <paramref name="onSuccessSelector"/>.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result<TReturn>> Select<TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, TReturn> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Select(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form as a
    /// <c>Success</c> result by passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is
    /// projected to the new form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the value returned by <paramref name="onSuccessSelector"/>.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result<TReturn>> Select<TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, Task<TReturn>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Select(onSuccessSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany(
        this Task<Result> sourceResult,
        Func<Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany(
        this Task<Result> sourceResult,
        Func<Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by passing
    /// its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Result> onSuccessSelector,
        Func<Result>? onNoneSelector = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector, onNoneSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by passing
    /// its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result>> onSuccessSelector,
        Func<Task<Result>>? onNoneSelector = null) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector, onNoneSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form
    /// as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Result<TReturn>> onSuccessSelector,
        Func<Result<TReturn>>? onNoneSelector = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector, onNoneSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form
    /// as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result<TReturn>>> onSuccessSelector,
        Func<Task<Result<TReturn>>>? onNoneSelector = null) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector, onNoneSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result> SelectMany(
        this Task<Result> sourceResult,
        Func<Unit, Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result> SelectMany(
        this Task<Result> sourceResult,
        Func<Unit, Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, Result> intermediateSelector,
        Func<Unit, Unit, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, Task<Result>> intermediateSelector,
        Func<Unit, Unit, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<TIntermediate, TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, Result<TIntermediate>> intermediateSelector,
        Func<Unit, TIntermediate, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<TIntermediate, TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, Task<Result<TIntermediate>>> intermediateSelector,
        Func<Unit, TIntermediate, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Result> intermediateSelector,
        Func<T, Unit, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result>> intermediateSelector,
        Func<T, Unit, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">A function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Result> ToFailIf(this Task<Result> sourceResult, Func<bool> predicate, Func<Error> getError) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFailIf(predicate, getError);

    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">A function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Result> ToFailIf(this Task<Result> sourceResult, Func<bool> predicate, Func<Task<Error>> getError) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFailIf(predicate, getError).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">A function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Result<T>> ToFailIf<T>(this Task<Result<T>> sourceResult, Func<T, bool> predicate, Func<T, Error> getError) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFailIf(predicate, getError);

    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">A function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Result<T>> ToFailIf<T>(this Task<Result<T>> sourceResult, Func<T, bool> predicate, Func<T, Task<Error>> getError) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFailIf(predicate, getError).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Gets a <c>Fail</c> result if the current result is <c>None</c>; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getError">A function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if the current result is <c>None</c>, or the same result if it is <c>Success</c> or
    ///     <c>Fail</c>.</returns>
    public static async Task<Result<T>> ToFailIfNone<T>(this Task<Result<T>> sourceResult, Func<Error> getError) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFailIfNone(getError);

    /// <summary>
    /// Gets a <c>Fail</c> result if the current result is <c>None</c>; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getError">A function that gets the <c>Fail</c> result's error.</param>
    /// <returns>A <c>Fail</c> result if the current result is <c>None</c>, or the same result if it is <c>Success</c> or
    ///     <c>Fail</c>.</returns>
    public static async Task<Result<T>> ToFailIfNone<T>(this Task<Result<T>> sourceResult, Func<Task<Error>> getError) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFailIfNone(getError).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Gets a <c>None</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">The delegate that determines whether to return a <c>None</c> result.</param>
    /// <returns>A <c>None</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Result<T>> ToNoneIf<T>(this Task<Result<T>> sourceResult, Func<T, bool> predicate) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToNoneIf(predicate);

    /// <summary>
    /// Truncates the <see cref="Result{T}"/> into an equivalent <see cref="Result"/>. If it is a <c>Success</c> result, then its
    /// value is ignored and a valueless <c>Success</c> result is returned. Otherwise, a <c>Fail</c> result with the same error
    /// as is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An equivalent <see cref="Result"/>.</returns>
    public static async Task<Result> Truncate<T>(this Task<Result<T>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Truncate();

    private class DisposableResult<TDisposable> : IDisposable
        where TDisposable : IDisposable
    {
        private readonly Result<TDisposable> _source;

        public DisposableResult(Result<TDisposable> source) => _source = source;

        public void Dispose() => _source.OnSuccess(value => value.Dispose());
    }

    private class AsyncDisposableResult<TAsyncDisposable> : IAsyncDisposable
        where TAsyncDisposable : IAsyncDisposable
    {
        private readonly Result<TAsyncDisposable> _source;

        public AsyncDisposableResult(Result<TAsyncDisposable> source) => _source = source;

        public async ValueTask DisposeAsync() => await _source.OnSuccess(value => value.DisposeAsync().AsTask()).ConfigureAwait(ContinueOnCapturedContext);
    }
}
