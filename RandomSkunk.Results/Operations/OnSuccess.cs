namespace RandomSkunk.Results;

/// <content> Defines the <c>OnSuccess</c> and <c>OnSuccessAsync</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public Result OnSuccess(Action onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_type == ResultType.Success)
            onSuccessCallback();

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Result> OnSuccessAsync(Func<Task> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_type == ResultType.Success)
            await onSuccessCallback().ConfigureAwait(false);

        return this;
    }
}

/// <content> Defines the <c>OnSuccess</c> and <c>OnSuccessAsync</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public Result<T> OnSuccess(Action<T> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_type == ResultType.Success)
            onSuccessCallback(_value!);

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Result<T>> OnSuccessAsync(Func<T, Task> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_type == ResultType.Success)
            await onSuccessCallback(_value!).ConfigureAwait(false);

        return this;
    }
}

/// <content> Defines the <c>OnSuccess</c> and <c>OnSuccessAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public Maybe<T> OnSuccess(Action<T> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_type == MaybeType.Success)
            onSuccessCallback(_value!);

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Maybe<T>> OnSuccessAsync(Func<T, Task> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_type == MaybeType.Success)
            await onSuccessCallback(_value!).ConfigureAwait(false);

        return this;
    }
}

/// <content> Defines the <c>OnSuccess</c> and <c>OnSuccessAsync</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnSuccess<T>(this Task<Maybe<T>> sourceResult, Action<T> onSuccessCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnSuccess(onSuccessCallback);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnSuccessAsync<T>(this Task<Maybe<T>> sourceResult, Func<T, Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnSuccessAsync(onSuccessCallback).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result> OnSuccess(this Task<Result> sourceResult, Action onSuccessCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnSuccess(onSuccessCallback);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result> OnSuccessAsync(this Task<Result> sourceResult, Func<Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnSuccessAsync(onSuccessCallback).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> sourceResult, Action<T> onSuccessCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnSuccess(onSuccessCallback);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnSuccessAsync<T>(this Task<Result<T>> sourceResult, Func<T, Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnSuccessAsync(onSuccessCallback).ConfigureAwait(false);
}
