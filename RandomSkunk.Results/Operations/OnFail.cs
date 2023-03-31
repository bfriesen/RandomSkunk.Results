namespace RandomSkunk.Results;

/// <content> Defines the <c>OnFail</c> methods. </content>
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

        if (_outcome == Outcome.Fail)
        {
            try
            {
                onFailCallback(GetError());
            }
            catch (Exception ex)
            {
                return Fail(CompositeError.CreateOrGetSingle(new[] { GetError(), Error.FromException(ex).InnerError! }));
            }
        }

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFailCallback">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Result> OnFail(Func<Error, Task> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                await onFailCallback(GetError()).ConfigureAwait(ContinueOnCapturedContext);
            }
            catch (Exception ex)
            {
                return Fail(CompositeError.CreateOrGetSingle(new[] { GetError(), Error.FromException(ex).InnerError! }));
            }
        }

        return this;
    }
}

/// <content> Defines the <c>OnFail</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFailCallback">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public Result<T> OnFail(Action<Error> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                onFailCallback(GetError());
            }
            catch (Exception ex)
            {
                return Fail(CompositeError.CreateOrGetSingle(new[] { GetError(), Error.FromException(ex).InnerError! }));
            }
        }

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFailCallback">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Result<T>> OnFail(Func<Error, Task> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                await onFailCallback(GetError()).ConfigureAwait(ContinueOnCapturedContext);
            }
            catch (Exception ex)
            {
                return Fail(CompositeError.CreateOrGetSingle(new[] { GetError(), Error.FromException(ex).InnerError! }));
            }
        }

        return this;
    }
}

/// <content> Defines the <c>OnFail</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFailCallback">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public Maybe<T> OnFail(Action<Error> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                onFailCallback(GetError());
            }
            catch (Exception ex)
            {
                return Fail(CompositeError.CreateOrGetSingle(new[] { GetError(), Error.FromException(ex).InnerError! }));
            }
        }

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFailCallback">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Maybe<T>> OnFail(Func<Error, Task> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                await onFailCallback(GetError()).ConfigureAwait(ContinueOnCapturedContext);
            }
            catch (Exception ex)
            {
                return Fail(CompositeError.CreateOrGetSingle(new[] { GetError(), Error.FromException(ex).InnerError! }));
            }
        }

        return this;
    }
}

/// <content> Defines the <c>OnFail</c> extension methods. </content>
public static partial class ResultExtensions
{
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
}
