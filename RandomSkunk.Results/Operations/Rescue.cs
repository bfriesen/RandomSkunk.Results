namespace RandomSkunk.Results;

/// <content> Defines the <c>Rescue</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned.
    /// </summary>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public Result Rescue(Func<Error, Result> onFail)
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                return onFail(GetError());
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onFail)));
            }
        }

        return this;
    }

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned.
    /// </summary>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public async Task<Result> Rescue(Func<Error, Task<Result>> onFail)
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                return await onFail(GetError());
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onFail)));
            }
        }

        return this;
    }
}

/// <content> Defines the <c>Rescue</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned.
    /// </summary>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public Result<T> Rescue(Func<Error, Result<T>> onFail)
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                return onFail(GetError());
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onFail)));
            }
        }

        return this;
    }

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned.
    /// </summary>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public async Task<Result<T>> Rescue(Func<Error, Task<Result<T>>> onFail)
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                return await onFail(GetError());
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onFail)));
            }
        }

        return this;
    }
}

/// <content> Defines the <c>Rescue</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned. Rescues a <c>None</c> result if the optional
    /// <paramref name="onNone"/> function is provided.
    /// </summary>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <param name="onNone">An optional function that rescues a <c>None</c> result.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public Maybe<T> Rescue(Func<Error, Maybe<T>> onFail, Func<Maybe<T>>? onNone = null)
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                return onFail(GetError());
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onFail)));
            }
        }
        else if (_outcome == Outcome.None && onNone is not null)
        {
            try
            {
                return onNone();
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onNone)));
            }
        }

        return this;
    }

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned. Rescues a <c>None</c> result if the optional
    /// <paramref name="onNone"/> function is provided.
    /// </summary>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <param name="onNone">An optional function that rescues a <c>None</c> result.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public async Task<Maybe<T>> Rescue(Func<Error, Task<Maybe<T>>> onFail, Func<Task<Maybe<T>>>? onNone = null)
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                return await onFail(GetError());
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onFail)));
            }
        }
        else if (_outcome == Outcome.None && onNone is not null)
        {
            try
            {
                return await onNone();
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onNone)));
            }
        }

        return this;
    }
}

/// <content> Defines the <c>Rescue</c> extension methods. </content>
public static partial class ResultExtensions
{
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
}
