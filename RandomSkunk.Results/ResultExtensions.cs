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
    /// <typeparam name="TDisposable">The <see cref="IDisposable"/> type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An <see cref="IDisposable"/> object.</returns>
    public static IDisposable AsDisposable<TDisposable>(this Maybe<TDisposable> sourceResult)
        where TDisposable : IDisposable =>
        new DisposableMaybe<TDisposable>(sourceResult);

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
    /// Gets an object that, when disposed, disposes the source result's value.
    /// </summary>
    /// <typeparam name="TAsyncDisposable">The <see cref="IAsyncDisposable"/> type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> object.</returns>
    public static IAsyncDisposable AsAsyncDisposable<TAsyncDisposable>(this Maybe<TAsyncDisposable> sourceResult)
        where TAsyncDisposable : IAsyncDisposable =>
        new AsyncDisposableMaybe<TAsyncDisposable>(sourceResult);

    /// <summary>
    /// Converts this <see cref="Result{T}"/> to an equivalent <see cref="Maybe{T}"/>. If this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned. Otherwise, if the <c>Fail</c> result's error has error code
    /// <see cref="ErrorCodes.NoValue"/>, then a <c>None</c> result is returned. For any other error code, a new <c>Fail</c>
    /// result with the same error is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public static async Task<Maybe<T>> AsMaybe<T>(this Task<Result<T>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).AsMaybe();

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
    public static Maybe<T> AsNonNullable<T>(this Maybe<T?> result)
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
    /// Gets an equivalent result with a non-nullable type.
    /// </summary>
    /// <typeparam name="T">The nullable type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <returns>The equivalant result.</returns>
    public static async Task<Maybe<T>> AsNonNullable<T>(this Task<Maybe<T?>> result)
        where T : struct =>
        (await result.ConfigureAwait(ContinueOnCapturedContext)).AsNonNullable();

    /// <summary>
    /// Converts this <see cref="Maybe{T}"/> to an equivalent <see cref="Result{T}"/>. If this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned. If this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned. Otherwise, if this is a <c>None</c> result, then a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NoValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public static async Task<Result<T>> AsResult<T>(this Task<Maybe<T>> sourceResult, Func<Result<T>>? onNoneSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).AsResult(onNoneSelector);

    /// <summary>
    /// Converts this <see cref="Maybe{T}"/> to an equivalent <see cref="Result{T}"/>. If this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned. If this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned. Otherwise, if this is a <c>None</c> result, then a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NoValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public static Task<Result<T>> AsResult<T>(this Task<Maybe<T>> sourceResult) =>
        sourceResult.AsResult(null);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Result> Else(this Task<Result> sourceResult, Result fallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(fallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or the value returned from <paramref name="getFallbackResult"/>.
    ///     </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public static async Task<Result> Else(this Task<Result> sourceResult, Func<Result> getFallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(getFallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Result<T>> Else<T>(this Task<Result<T>> sourceResult, Result<T> fallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(fallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or the value returned from <paramref name="getFallbackResult"/>.
    ///     </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> Else<T>(this Task<Result<T>> sourceResult, Func<Result<T>> getFallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(getFallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Maybe<T>> Else<T>(this Task<Maybe<T>> sourceResult, Maybe<T> fallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(fallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or the value returned from <paramref name="getFallbackResult"/>.
    ///     </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Else<T>(this Task<Maybe<T>> sourceResult, Func<Maybe<T>> getFallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(getFallbackResult);

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
    public static Maybe<T> Flatten<T>(this Result<Maybe<T>> sourceResult) =>
        sourceResult.SelectMany(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> sourceResult) =>
        sourceResult.SelectMany(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result Flatten(this Maybe<Result> sourceResult) =>
        sourceResult.SelectMany(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result<T> Flatten<T>(this Maybe<Result<T>> sourceResult) =>
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
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Maybe<T>> Flatten<T>(this Task<Result<Maybe<T>>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Flatten();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Maybe<T>> Flatten<T>(this Task<Maybe<Maybe<T>>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Flatten();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Result> Flatten(this Task<Maybe<Result>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Flatten();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Result<T>> Flatten<T>(this Task<Maybe<Result<T>>> sourceResult) =>
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
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> or <c>None</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackValue">The fallback value to return if this is not a <c>Success</c> result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, <paramref name="fallbackValue"/>.
    ///     </returns>
    public static async Task<T?> GetValueOr<T>(this Task<Maybe<T>> sourceResult, T? fallbackValue) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).GetValueOr(fallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> or <c>None</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackValue">A function that creates the fallback value to return if this is not a <c>Success</c>
    ///     result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the value returned by the
    ///     <paramref name="getFallbackValue"/> function.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<T?> GetValueOr<T>(this Task<Maybe<T>> sourceResult, Func<T?> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).GetValueOr(getFallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the default value of type <typeparamref name="T"/> if it is a <c>Fail</c>
    /// or <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the default value of type
    ///     <typeparamref name="T"/>.</returns>
    public static async Task<T?> GetValueOrDefault<T>(this Task<Maybe<T>> sourceResult) =>
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
        this Task<Maybe<T>> sourceResult,
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
        this Task<Maybe<T>> sourceResult,
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
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnFail<T>(this Task<Maybe<T>> sourceResult, Action<Error> onFailCallback) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnFail(onFailCallback);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnFail<T>(this Task<Maybe<T>> sourceResult, Func<Error, Task> onFailCallback) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnFail(onFailCallback).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Invokes the <paramref name="onNoneCallback"/> function if <paramref name="sourceResult"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneCallback">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnNone<T>(this Task<Maybe<T>> sourceResult, Action onNoneCallback) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnNone(onNoneCallback);

    /// <summary>
    /// Invokes the <paramref name="onNoneCallback"/> function if <paramref name="sourceResult"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneCallback">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnNone<T>(this Task<Maybe<T>> sourceResult, Func<Task> onNoneCallback) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnNone(onNoneCallback).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Invokes the <paramref name="onNonSuccess"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccess">A callback function to invoke if this is a <c>non-Success</c> result.</param>
    /// <returns>The current result.</returns>
    public static TResult OnNonSuccess<TResult>(
        this TResult sourceResult,
        Action<Error> onNonSuccess)
        where TResult : IResult
    {
        if (onNonSuccess is null) throw new ArgumentNullException(nameof(onNonSuccess));

        if (!sourceResult.IsSuccess)
        {
            var error = sourceResult.GetNonSuccessError();
            onNonSuccess(error);
        }

        return sourceResult;
    }

    /// <summary>
    /// Invokes the <paramref name="onNonSuccess"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccess">A callback function to invoke if this is a <c>non-Success</c> result.</param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnNonSuccess<TResult>(
        this TResult sourceResult,
        Func<Error, Task> onNonSuccess)
        where TResult : IResult
    {
        if (onNonSuccess is null) throw new ArgumentNullException(nameof(onNonSuccess));

        if (!sourceResult.IsSuccess)
        {
            var error = sourceResult.GetNonSuccessError();
            await onNonSuccess(error).ConfigureAwait(ContinueOnCapturedContext);
        }

        return sourceResult;
    }

    /// <summary>
    /// Invokes the <paramref name="onNonSuccessCallback"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccessCallback">A callback function to invoke if the source is a <c>non-Success</c> result.</param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnNonSuccess<TResult>(
        this Task<TResult> sourceResult,
        Action<Error> onNonSuccessCallback)
        where TResult : IResult =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnNonSuccess(onNonSuccessCallback);

    /// <summary>
    /// Invokes the <paramref name="onNonSuccess"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccess">A callback function to invoke if the source is a <c>non-Success</c> result.</param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnNonSuccess<TResult>(
        this Task<TResult> sourceResult,
        Func<Error, Task> onNonSuccess)
        where TResult : IResult =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnNonSuccess(onNonSuccess).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnSuccess<T>(this Task<Maybe<T>> sourceResult, Action<T> onSuccessCallback) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnSuccess(onSuccessCallback);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnSuccess<T>(this Task<Maybe<T>> sourceResult, Func<T, Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnSuccess(onSuccessCallback).ConfigureAwait(ContinueOnCapturedContext);

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
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result; otherwise, returns a new <c>Success</c> result
    /// with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="fallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> Or<T>(this Task<Result<T>> sourceResult, [DisallowNull] T fallbackValue) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Or(fallbackValue);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result; otherwise, returns a new <c>Success</c> result
    /// with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackValue">A function that returns the fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> Or<T>(this Task<Result<T>> sourceResult, Func<T> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Or(getFallbackValue);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result; otherwise, returns a new <c>Success</c> result
    /// with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="fallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Or<T>(this Task<Maybe<T>> sourceResult, [DisallowNull] T fallbackValue) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Or(fallbackValue);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result; otherwise, returns a new <c>Success</c> result
    /// with its value from evaluating the <paramref name="getFallbackValue"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackValue">A function that returns the fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Or<T>(this Task<Maybe<T>> sourceResult, Func<T> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Or(getFallbackValue);

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
    /// <c>Success</c>, nothing happens and the current result is returned. Rescues a <c>None</c> result if the optional
    /// <paramref name="onNone"/> function is provided.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <param name="onNone">An optional function that rescues a <c>None</c> result.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public static async Task<Maybe<T>> Rescue<T>(
        this Task<Maybe<T>> sourceResult,
        Func<Error, Maybe<T>> onFail,
        Func<Maybe<T>>? onNone = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Rescue(onFail, onNone);

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned. Rescues a <c>None</c> result if the optional
    /// <paramref name="onNone"/> function is provided.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <param name="onNone">An optional function that rescues a <c>None</c> result.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public static async Task<Maybe<T>> Rescue<T>(
        this Task<Maybe<T>> sourceResult,
        Func<Error, Task<Maybe<T>>> onFail,
        Func<Task<Maybe<T>>>? onNone = null) =>
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
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> Select<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, TReturn> onSuccessSelector) =>
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
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the value returned by <paramref name="onSuccessSelector"/>.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> Select<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Select(onSuccessSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form as a
    /// <c>Success</c> result by passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is
    /// projected to the new form as a <c>Fail</c> result with the same error; a <c>None</c> result is projected to the new form
    /// as a <c>None</c> result.
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
    public static async Task<Maybe<TReturn>> Select<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, TReturn> onSuccessSelector,
        Func<TReturn>? onNoneSelector = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Select(onSuccessSelector, onNoneSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form as a
    /// <c>Success</c> result by passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is
    /// projected to the new form as a <c>Fail</c> result with the same error; a <c>None</c> result is projected to the new form
    /// as a <c>None</c> result.
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
    public static async Task<Maybe<TReturn>> Select<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
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
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by passing
    /// its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by passing
    /// its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector).ConfigureAwait(ContinueOnCapturedContext);

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
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector);

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
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form by
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
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form by
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
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by passing
    /// its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error; a <c>None</c> result is projected to the new form as a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.NoValue"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result> onSuccessSelector,
        Func<Result>? onNoneSelector = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector, onNoneSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by passing
    /// its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error; a <c>None</c> result is projected to the new form as a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.NoValue"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result>> onSuccessSelector,
        Func<Task<Result>>? onNoneSelector = null) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector, onNoneSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form
    /// as a <c>Fail</c> result with the same error; a <c>None</c> result is projected to the new form as a <c>Fail</c> result
    /// with error code <see cref="ErrorCodes.NoValue"/>.
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
        this Task<Maybe<T>> sourceResult,
        Func<T, Result<TReturn>> onSuccessSelector,
        Func<Result<TReturn>>? onNoneSelector = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector, onNoneSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form
    /// as a <c>Fail</c> result with the same error; a <c>None</c> result is projected to the new form as a <c>Fail</c> result
    /// with error code <see cref="ErrorCodes.NoValue"/>.
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
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result<TReturn>>> onSuccessSelector,
        Func<Task<Result<TReturn>>>? onNoneSelector = null) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector, onNoneSelector).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form
    /// as a <c>Fail</c> result with the same error; a <c>None</c> result is projected to the new form as a <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Maybe<TReturn>> onSuccessSelector,
        Func<Maybe<TReturn>>? onNoneSelector = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector, onNoneSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form
    /// as a <c>Fail</c> result with the same error; a <c>None</c> result is projected to the new form as a <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Maybe<TReturn>>> onSuccessSelector,
        Func<Task<Maybe<TReturn>>>? onNoneSelector = null) =>
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
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Maybe<TIntermediate>> intermediateSelector,
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
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Maybe<TIntermediate>>> intermediateSelector,
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
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
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
        this Task<Maybe<T>> sourceResult,
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
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Maybe<TIntermediate>> intermediateSelector,
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
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
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
        this Task<Maybe<T>> sourceResult,
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
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Maybe<TIntermediate>>> intermediateSelector,
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
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Maybe<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Maybe<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, Task<Maybe<TReturn>>> onSuccessSelector) =>
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
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<TIntermediate, TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, Maybe<TIntermediate>> intermediateSelector,
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
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<TIntermediate, TReturn>(
        this Task<Result> sourceResult,
        Func<Unit, Task<Maybe<TIntermediate>>> intermediateSelector,
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
        this Task<Maybe<T>> sourceResult,
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
        this Task<Maybe<T>> sourceResult,
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
    /// <param name="predicate">The delegate that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">An optional function that gets the <c>Fail</c> result's error. If <see langword="null"/>, a
    ///     generic error is used.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Result> ToFailIf(this Task<Result> sourceResult, Func<bool> predicate, Func<Error>? getError = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFailIf(predicate, getError);

    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">The delegate that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">An optional function that gets the <c>Fail</c> result's error. If <see langword="null"/>, a
    ///     generic error is used.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Result<T>> ToFailIf<T>(this Task<Result<T>> sourceResult, Func<T, bool> predicate, Func<T, Error>? getError = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFailIf(predicate, getError);

    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">The delegate that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">An optional function that gets the <c>Fail</c> result's error. If <see langword="null"/>, a
    ///     generic error is used.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Maybe<T>> ToFailIf<T>(this Task<Maybe<T>> sourceResult, Func<T, bool> predicate, Func<T, Error>? getError = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFailIf(predicate, getError);

    /// <summary>
    /// Gets a <c>Fail</c> result if the current result is <c>None</c>; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getError">An optional function that gets the <c>Fail</c> result's error. If <see langword="null"/>, a
    ///     generic error is used.</param>
    /// <returns>A <c>Fail</c> result if the current result is <c>None</c>, or the same result if it is <c>Success</c> or
    ///     <c>Fail</c>.</returns>
    public static async Task<Maybe<T>> ToFailIfNone<T>(this Task<Maybe<T>> sourceResult, Func<Error>? getError = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ToFailIfNone(getError);

    /// <summary>
    /// Gets a <c>None</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">The delegate that determines whether to return a <c>None</c> result.</param>
    /// <returns>A <c>None</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public static async Task<Maybe<T>> ToNoneIf<T>(this Task<Maybe<T>> sourceResult, Func<T, bool> predicate) =>
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

    /// <summary>
    /// Truncates the <see cref="Maybe{T}"/> into an equivalent <see cref="Result"/>. If it is a <c>Success</c> result, then its
    /// value is ignored and a valueless <c>Success</c> result is returned. If it is a <c>None</c> result, then a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoValue"/> is returned. Otherwise, a <c>Fail</c> result with the same error
    /// as is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>An equivalent <see cref="Result"/>.</returns>
    public static async Task<Result> Truncate<T>(this Task<Maybe<T>> sourceResult, Func<Result>? onNoneSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Truncate(onNoneSelector);

    /// <summary>
    /// Truncates the <see cref="Maybe{T}"/> into an equivalent <see cref="Result"/>. If it is a <c>Success</c> result, then its
    /// value is ignored and a valueless <c>Success</c> result is returned. If it is a <c>None</c> result, then a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoValue"/> is returned. Otherwise, a <c>Fail</c> result with the same error
    /// as is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An equivalent <see cref="Result"/>.</returns>
    public static Task<Result> Truncate<T>(this Task<Maybe<T>> sourceResult) =>
        sourceResult.Truncate(null);

    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceResult"/> is <see langword="null"/> or if
    ///     <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Result<T>> sourceResult,
        Func<T, bool> predicate) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Where(predicate);

    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceResult"/> is <see langword="null"/> or if
    ///     <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<bool>> predicate) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Where(predicate).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceResult"/> is <see langword="null"/> or if
    ///     <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, bool> predicate) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Where(predicate);

    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceResult"/> is <see langword="null"/> or if
    ///     <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Where<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<bool>> predicate) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Where(predicate).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, <paramref name="sourceResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public static async Task<Result> WithError(this Task<Result> sourceResult, Func<Error, Error> onFailGetError) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).WithError(onFailGetError);

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, <paramref name="sourceResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> WithError<T>(this Task<Result<T>> sourceResult, Func<Error, Error> onFailGetError) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).WithError(onFailGetError);

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, <paramref name="sourceResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> WithError<T>(this Task<Maybe<T>> sourceResult, Func<Error, Error> onFailGetError) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).WithError(onFailGetError);

    private class DisposableResult<TDisposable> : IDisposable
        where TDisposable : IDisposable
    {
        private readonly Result<TDisposable> _source;

        public DisposableResult(Result<TDisposable> source) => _source = source;

        public void Dispose() => _source.OnSuccess(value => value.Dispose());
    }

    private class DisposableMaybe<TDisposable> : IDisposable
        where TDisposable : IDisposable
    {
        private readonly Maybe<TDisposable> _source;

        public DisposableMaybe(Maybe<TDisposable> source) => _source = source;

        public void Dispose() => _source.OnSuccess(value => value.Dispose());
    }

    private class AsyncDisposableResult<TAsyncDisposable> : IAsyncDisposable
        where TAsyncDisposable : IAsyncDisposable
    {
        private readonly Result<TAsyncDisposable> _source;

        public AsyncDisposableResult(Result<TAsyncDisposable> source) => _source = source;

        public async ValueTask DisposeAsync() => await _source.OnSuccess(value => value.DisposeAsync().AsTask()).ConfigureAwait(ContinueOnCapturedContext);
    }

    private class AsyncDisposableMaybe<TAsyncDisposable> : IAsyncDisposable
        where TAsyncDisposable : IAsyncDisposable
    {
        private readonly Maybe<TAsyncDisposable> _source;

        public AsyncDisposableMaybe(Maybe<TAsyncDisposable> source) => _source = source;

        public async ValueTask DisposeAsync() => await _source.OnSuccess(value => value.DisposeAsync().AsTask()).ConfigureAwait(ContinueOnCapturedContext);
    }
}
