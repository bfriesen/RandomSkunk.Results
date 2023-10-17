namespace RandomSkunk.Results;

/// <summary>
/// Defines methods that evaluate a delegate using a try-catch statement with a two catch blocks: the first has an exception of
/// type <typeparamref name="TException1"/> and the second has an exception of type <typeparamref name="TException2"/>.
/// </summary>
/// <typeparam name="TException1">The first type of exception to catch.</typeparam>
/// <typeparam name="TException2">The second type of exception to catch.</typeparam>
public static class TryCatch<TException1, TException2>
    where TException1 : Exception
    where TException2 : Exception
{
    private static readonly Func<TException1, Error> _defaultException1Handler = TryCatch.GetErrorFromException;
    private static readonly Func<TException2, Error> _defaultException2Handler = TryCatch.GetErrorFromException;

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, a <c>Success</c> result is returned. If the
    /// delegate throws an exception of type <typeparamref name="TException1"/> or <typeparamref name="TException2"/>, it is
    /// caught and used to create the error for the returned <c>Fail</c> result. Otherwise, if the delegate throws some other
    /// exception, it is not caught.
    /// </summary>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exception1Handler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="exception2Handler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result AsResult(
        Action sourceDelegate,
        Func<TException1, Error>? exception1Handler = null,
        Func<TException2, Error>? exception2Handler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        if (exception1Handler is null) exception1Handler = _defaultException1Handler;
        if (exception2Handler is null) exception2Handler = _defaultException2Handler;

        try
        {
            sourceDelegate();
            return Result.Success();
        }
        catch (TaskCanceledException ex)
        {
            return Errors.Canceled(ex);
        }
        catch (TException1 ex)
        {
            return exception1Handler(ex);
        }
        catch (TException2 ex)
        {
            return exception2Handler(ex);
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, a <c>Success</c> result is returned. If the
    /// delegate throws an exception of type <typeparamref name="TException1"/> or <typeparamref name="TException2"/>, it is
    /// caught and used to create the error for the returned <c>Fail</c> result. Otherwise, if the delegate throws some other
    /// exception, it is not caught.
    /// </summary>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exception1Handler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="exception2Handler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result> AsResult(
        Func<Task> sourceDelegate,
        Func<TException1, Error>? exception1Handler = null,
        Func<TException2, Error>? exception2Handler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        if (exception1Handler is null) exception1Handler = _defaultException1Handler;
        if (exception2Handler is null) exception2Handler = _defaultException2Handler;

        try
        {
            await sourceDelegate().ConfigureAwait(ContinueOnCapturedContext);
            return Result.Success();
        }
        catch (TaskCanceledException ex)
        {
            return Errors.Canceled(ex);
        }
        catch (TException1 ex)
        {
            return exception1Handler(ex);
        }
        catch (TException2 ex)
        {
            return exception2Handler(ex);
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. If the delegate throws an exception of type <typeparamref name="TException1"/> or
    /// <typeparamref name="TException2"/>, it is caught and used to create the error for the returned <c>Fail</c> result.
    /// Otherwise, if the delegate throws some other exception, it is not caught.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exception1Handler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="exception2Handler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result<T> AsResult<T>(
        Func<T?> sourceDelegate,
        Func<TException1, Error>? exception1Handler = null,
        Func<TException2, Error>? exception2Handler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        if (exception1Handler is null) exception1Handler = _defaultException1Handler;
        if (exception2Handler is null) exception2Handler = _defaultException2Handler;

        try
        {
            var value = sourceDelegate();
            return value;
        }
        catch (TaskCanceledException ex)
        {
            return Errors.Canceled(ex);
        }
        catch (TException1 ex)
        {
            return exception1Handler(ex);
        }
        catch (TException2 ex)
        {
            return exception2Handler(ex);
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. If the delegate throws an exception of type <typeparamref name="TException1"/> or
    /// <typeparamref name="TException2"/>, it is caught and used to create the error for the returned <c>Fail</c> result.
    /// Otherwise, if the delegate throws some other exception, it is not caught.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exception1Handler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="exception2Handler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> AsResult<T>(
        Func<Task<T>> sourceDelegate,
        Func<TException1, Error>? exception1Handler = null,
        Func<TException2, Error>? exception2Handler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        if (exception1Handler is null) exception1Handler = _defaultException1Handler;
        if (exception2Handler is null) exception2Handler = _defaultException2Handler;

        try
        {
            var value = await sourceDelegate().ConfigureAwait(ContinueOnCapturedContext);
            return value;
        }
        catch (TaskCanceledException ex)
        {
            return Errors.Canceled(ex);
        }
        catch (TException1 ex)
        {
            return exception1Handler(ex);
        }
        catch (TException2 ex)
        {
            return exception2Handler(ex);
        }
    }
}
