namespace RandomSkunk.Results;

/// <content> Defines the <c>Select</c> methods. </content>
public partial struct Result
{
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
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Result<TReturn> Select<TReturn>(Func<Unit, TReturn> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
        {
            try
            {
                return onSuccessSelector(Unit.Value).ToResult();
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Result<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        return Result<TReturn>.Fail(GetError(), true);
    }

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
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public async Task<Result<TReturn>> Select<TReturn>(Func<Unit, Task<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
        {
            try
            {
                return (await onSuccessSelector(Unit.Value).ConfigureAwait(ContinueOnCapturedContext)).ToResult();
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Result<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        return Result<TReturn>.Fail(GetError(), true);
    }
}

/// <content> Defines the <c>Select</c> methods. </content>
public partial struct Result<T>
{
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
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> Select<TReturn>(Func<T, TReturn> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
        {
            try
            {
                return onSuccessSelector(_value!).ToResult();
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Result<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        return Result<TReturn>.Fail(GetError(), true);
    }

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
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> Select<TReturn>(Func<T, Task<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
        {
            try
            {
                return (await onSuccessSelector(_value!).ConfigureAwait(ContinueOnCapturedContext)).ToResult();
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Result<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        return Result<TReturn>.Fail(GetError(), true);
    }
}

/// <content> Defines the <c>Select</c> methods. </content>
public partial struct Maybe<T>
{
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
    /// <typeparam name="TReturn">The type of the value returned by <paramref name="onSuccessSelector"/>.</typeparam>
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> Select<TReturn>(
        Func<T, TReturn> onSuccessSelector,
        Func<TReturn>? onNoneSelector = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
        {
            try
            {
                return onSuccessSelector(_value!).ToMaybe();
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Maybe<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        if (_outcome == Outcome.None)
        {
            if (onNoneSelector is null)
                return Maybe<TReturn>.None;

            try
            {
                return onNoneSelector().ToMaybe();
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Maybe<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onNoneSelector)));
            }
        }

        return Maybe<TReturn>.Fail(GetError(), true);
    }

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
    /// <typeparam name="TReturn">The type of the value returned by <paramref name="onSuccessSelector"/>.</typeparam>
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> Select<TReturn>(
        Func<T, Task<TReturn>> onSuccessSelector,
        Func<Task<TReturn>>? onNoneSelector = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
        {
            try
            {
                return (await onSuccessSelector(_value!).ConfigureAwait(ContinueOnCapturedContext)).ToMaybe();
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Maybe<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        if (_outcome == Outcome.None)
        {
            if (onNoneSelector is null)
                return Maybe<TReturn>.None;

            try
            {
                return (await onNoneSelector().ConfigureAwait(ContinueOnCapturedContext)).ToMaybe();
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Maybe<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onNoneSelector)));
            }
        }

        return Maybe<TReturn>.Fail(GetError(), true);
    }
}

/// <content> Defines the <c>Select</c> extension methods. </content>
public static partial class ResultExtensions
{
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
}
