namespace RandomSkunk.Results;

/// <content> Defines the <c>OnSuccess</c> methods. </content>
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

        if (_outcome == _successOutcome)
            onSuccessCallback();

        return this;
    }

    /// <inheritdoc cref="OnSuccess(Action)"/>
    public async Task<Result> OnSuccess(Func<Task> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_outcome == _successOutcome)
            await onSuccessCallback().ConfigureAwait(false);

        return this;
    }
}

/// <content> Defines the <c>OnSuccess</c> methods. </content>
public partial struct Result<T>
{
    /// <inheritdoc cref="Result.OnSuccess(Action)"/>
    public Result<T> OnSuccess(Action<T> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_outcome == _successOutcome)
            onSuccessCallback(_value!);

        return this;
    }

    /// <inheritdoc cref="Result.OnSuccess(Action)"/>
    public async Task<Result<T>> OnSuccess(Func<T, Task> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_outcome == _successOutcome)
            await onSuccessCallback(_value!).ConfigureAwait(false);

        return this;
    }
}

/// <content> Defines the <c>OnSuccess</c> methods. </content>
public partial struct Maybe<T>
{
    /// <inheritdoc cref="Result.OnSuccess(Action)"/>
    public Maybe<T> OnSuccess(Action<T> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_outcome == _successOutcome)
            onSuccessCallback(_value!);

        return this;
    }

    /// <inheritdoc cref="Result.OnSuccess(Action)"/>
    public async Task<Maybe<T>> OnSuccess(Func<T, Task> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_outcome == _successOutcome)
            await onSuccessCallback(_value!).ConfigureAwait(false);

        return this;
    }
}

/// <content> Defines the <c>OnSuccess</c> extension methods. </content>
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

    /// <inheritdoc cref="OnSuccess{T}(Task{Maybe{T}}, Action{T})"/>
    public static async Task<Maybe<T>> OnSuccess<T>(this Task<Maybe<T>> sourceResult, Func<T, Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnSuccess(onSuccessCallback).ConfigureAwait(false);

    /// <inheritdoc cref="OnSuccess{T}(Task{Maybe{T}}, Action{T})"/>
    public static async Task<Result> OnSuccess(this Task<Result> sourceResult, Action onSuccessCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnSuccess(onSuccessCallback);

    /// <inheritdoc cref="OnSuccess{T}(Task{Maybe{T}}, Action{T})"/>
    public static async Task<Result> OnSuccess(this Task<Result> sourceResult, Func<Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnSuccess(onSuccessCallback).ConfigureAwait(false);

    /// <inheritdoc cref="OnSuccess{T}(Task{Maybe{T}}, Action{T})"/>
    public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> sourceResult, Action<T> onSuccessCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnSuccess(onSuccessCallback);

    /// <inheritdoc cref="OnSuccess{T}(Task{Maybe{T}}, Action{T})"/>
    public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> sourceResult, Func<T, Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnSuccess(onSuccessCallback).ConfigureAwait(false);
}
