namespace RandomSkunk.Results;

/// <summary>
/// Defines methods that evaluate a delegate using a try-catch statement with a single catch block with exception of type
/// <typeparamref name="TException"/>.
/// </summary>
/// <typeparam name="TException">The type of exception to catch.</typeparam>
public static class TryCatch<TException>
    where TException : Exception
{
    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, a <c>Success</c> result is returned. If the
    /// delegate throws an exception of type <typeparamref name="TException"/>, it is caught and used to create the error for the
    /// returned <c>Fail</c> result. Otherwise, if the delegate throws some other exception, it is not caught.
    /// </summary>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result AsResult(
        Action sourceDelegate,
        Func<TException, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        try
        {
            sourceDelegate();
            return Result.Success();
        }
        catch (TaskCanceledException ex)
        {
            return Errors.Canceled(ex);
        }
        catch (TException ex)
        {
            exceptionHandler ??= TryCatch.DefaultExceptionHandler;
            return Result.Fail(exceptionHandler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, a <c>Success</c> result is returned. If the
    /// delegate throws an exception of type <typeparamref name="TException"/>, it is caught and used to create the error for the
    /// returned <c>Fail</c> result. Otherwise, if the delegate throws some other exception, it is not caught.
    /// </summary>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result> AsResult(
        Func<Task> sourceDelegate,
        Func<TException, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        try
        {
            await sourceDelegate().ConfigureAwait(ContinueOnCapturedContext);
            return Result.Success();
        }
        catch (TaskCanceledException ex)
        {
            return Errors.Canceled(ex);
        }
        catch (TException ex)
        {
            exceptionHandler ??= TryCatch.DefaultExceptionHandler;
            return Result.Fail(exceptionHandler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. If the delegate throws an exception of type <typeparamref name="TException"/>, it is caught and used to
    /// create the error for the returned <c>Fail</c> result. Otherwise, if the delegate throws some other exception, it is not
    /// caught.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result<T> AsResult<T>(
        Func<T?> sourceDelegate,
        Func<TException, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        try
        {
            var value = sourceDelegate();
            return Result<T>.FromValue(value);
        }
        catch (TaskCanceledException ex)
        {
            return Errors.Canceled(ex);
        }
        catch (TException ex)
        {
            exceptionHandler ??= TryCatch.DefaultExceptionHandler;
            return Result<T>.Fail(exceptionHandler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. If the delegate throws an exception of type <typeparamref name="TException"/>, it is caught and used to
    /// create the error for the returned <c>Fail</c> result. Otherwise, if the delegate throws some other exception, it is not
    /// caught.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> AsResult<T>(
        Func<Task<T>> sourceDelegate,
        Func<TException, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        try
        {
            var value = await sourceDelegate().ConfigureAwait(ContinueOnCapturedContext);
            return Result<T>.FromValue(value);
        }
        catch (TaskCanceledException ex)
        {
            return Errors.Canceled(ex);
        }
        catch (TException ex)
        {
            exceptionHandler ??= TryCatch.DefaultExceptionHandler;
            return Result<T>.Fail(exceptionHandler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Maybe{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. If the delegate throws an exception of type <typeparamref name="TException"/>, it is caught and used to
    /// create the error for the returned <c>Fail</c> result. Otherwise, if the delegate throws some other exception, it is not
    /// caught.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Maybe<T> AsMaybe<T>(
        Func<T?> sourceDelegate,
        Func<TException, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));


        try
        {
            var value = sourceDelegate();
            return Maybe<T>.FromValue(value);
        }
        catch (TaskCanceledException ex)
        {
            return Errors.Canceled(ex);
        }
        catch (TException ex)
        {
            exceptionHandler ??= TryCatch.DefaultExceptionHandler;
            return Maybe<T>.Fail(exceptionHandler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Maybe{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. If the delegate throws an exception of type <typeparamref name="TException"/>, it is caught and used to
    /// create the error for the returned <c>Fail</c> result. Otherwise, if the delegate throws some other exception, it is not
    /// caught.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> AsMaybe<T>(
        Func<Task<T>> sourceDelegate,
        Func<TException, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        try
        {
            var value = await sourceDelegate().ConfigureAwait(ContinueOnCapturedContext);
            return Maybe<T>.FromValue(value);
        }
        catch (TaskCanceledException ex)
        {
            return Errors.Canceled(ex);
        }
        catch (TException ex)
        {
            exceptionHandler ??= TryCatch.DefaultExceptionHandler;
            return Maybe<T>.Fail(exceptionHandler(ex));
        }
    }
}
