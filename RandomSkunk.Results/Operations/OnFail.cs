namespace RandomSkunk.Results;

/// <content> Defines the <c>OnFail</c> and <c>OnFailAsync</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFailCallback">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public Result OnFail(Action<Error> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == _failOutcome)
            onFailCallback(GetError());

        return this;
    }

    /// <inheritdoc cref="OnFail(Action{Error})"/>
    public async Task<Result> OnFailAsync(Func<Error, Task> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == _failOutcome)
            await onFailCallback(GetError()).ConfigureAwait(false);

        return this;
    }
}

/// <content> Defines the <c>OnFail</c> and <c>OnFailAsync</c> methods. </content>
public partial struct Result<T>
{
    /// <inheritdoc cref="Result.OnFail(Action{Error})"/>
    public Result<T> OnFail(Action<Error> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == _failOutcome)
            onFailCallback(GetError());

        return this;
    }

    /// <inheritdoc cref="Result.OnFail(Action{Error})"/>
    public async Task<Result<T>> OnFailAsync(Func<Error, Task> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == _failOutcome)
            await onFailCallback(GetError()).ConfigureAwait(false);

        return this;
    }
}

/// <content> Defines the <c>OnFail</c> and <c>OnFailAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <inheritdoc cref="Result.OnFail(Action{Error})"/>
    public Maybe<T> OnFail(Action<Error> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == _failOutcome)
            onFailCallback(GetError());

        return this;
    }

    /// <inheritdoc cref="Result.OnFail(Action{Error})"/>
    public async Task<Maybe<T>> OnFailAsync(Func<Error, Task> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == _failOutcome)
            await onFailCallback(GetError()).ConfigureAwait(false);

        return this;
    }
}

/// <content> Defines the <c>OnFail</c> and <c>OnFailAsync</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <inheritdoc cref="OnFail{T}(Task{Result{T}}, Action{Error})"/>
    public static async Task<Result> OnFail(this Task<Result> sourceResult, Action<Error> onFailCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnFail(onFailCallback);

    /// <inheritdoc cref="OnFail{T}(Task{Result{T}}, Action{Error})"/>
    public static async Task<Result> OnFailAsync(this Task<Result> sourceResult, Func<Error, Task> onFailCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnFailAsync(onFailCallback).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnFail<T>(this Task<Result<T>> sourceResult, Action<Error> onFailCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnFail(onFailCallback);

    /// <inheritdoc cref="OnFail{T}(Task{Result{T}}, Action{Error})"/>
    public static async Task<Result<T>> OnFailAsync<T>(this Task<Result<T>> sourceResult, Func<Error, Task> onFailCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnFailAsync(onFailCallback).ConfigureAwait(false);

    /// <inheritdoc cref="OnFail{T}(Task{Result{T}}, Action{Error})"/>
    public static async Task<Maybe<T>> OnFail<T>(this Task<Maybe<T>> sourceResult, Action<Error> onFailCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnFail(onFailCallback);

    /// <inheritdoc cref="OnFail{T}(Task{Result{T}}, Action{Error})"/>
    public static async Task<Maybe<T>> OnFailAsync<T>(this Task<Maybe<T>> sourceResult, Func<Error, Task> onFailCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnFailAsync(onFailCallback).ConfigureAwait(false);
}
