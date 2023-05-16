namespace RandomSkunk.Results;

/// <summary>
/// Defines methods that evaluate a delegate using a try-catch statement with a four catch blocks: the first has an exception of
/// type <typeparamref name="TException1"/>, the second has an exception of type <typeparamref name="TException2"/>, the third
/// has an exception of type <typeparamref name="TException3"/>, and the fourth has an exception of type
/// <typeparamref name="TException4"/>.
/// </summary>
/// <typeparam name="TException1">The first type of exception to catch.</typeparam>
/// <typeparam name="TException2">The second type of exception to catch.</typeparam>
/// <typeparam name="TException3">The third type of exception to catch.</typeparam>
/// <typeparam name="TException4">The fourth type of exception to catch.</typeparam>
public static class TryCatch<TException1, TException2, TException3, TException4>
    where TException1 : Exception
    where TException2 : Exception
    where TException3 : Exception
    where TException4 : Exception
{
    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, a <c>Success</c> result is returned. If the
    /// delegate throws an exception of type <typeparamref name="TException1"/>, <typeparamref name="TException2"/>,
    /// <typeparamref name="TException3"/>, or <typeparamref name="TException4"/>, it is caught and used to create the error for
    /// the returned <c>Fail</c> result. Otherwise, if the delegate throws some other exception, it is not caught.
    /// </summary>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exception1Handler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception2Handler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception3Handler">An optional function that maps a caught <typeparamref name="TException3"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception4Handler">An optional function that maps a caught <typeparamref name="TException4"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result AsResult(
        Action sourceDelegate,
        Func<TException1, Error>? exception1Handler = null,
        Func<TException2, Error>? exception2Handler = null,
        Func<TException3, Error>? exception3Handler = null,
        Func<TException4, Error>? exception4Handler = null)
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
        catch (TException1 ex)
        {
            exception1Handler ??= TryCatch.DefaultExceptionHandler;
            return Result.Fail(exception1Handler(ex));
        }
        catch (TException2 ex)
        {
            exception2Handler ??= TryCatch.DefaultExceptionHandler;
            return Result.Fail(exception2Handler(ex));
        }
        catch (TException3 ex)
        {
            exception3Handler ??= TryCatch.DefaultExceptionHandler;
            return Result.Fail(exception3Handler(ex));
        }
        catch (TException4 ex)
        {
            exception4Handler ??= TryCatch.DefaultExceptionHandler;
            return Result.Fail(exception4Handler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, a <c>Success</c> result is returned. If the
    /// delegate throws an exception of type <typeparamref name="TException1"/>, <typeparamref name="TException2"/>,
    /// <typeparamref name="TException3"/>, or <typeparamref name="TException4"/>, it is caught and used to create the error for
    /// the returned <c>Fail</c> result. Otherwise, if the delegate throws some other exception, it is not caught.
    /// </summary>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exception1Handler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception2Handler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception3Handler">An optional function that maps a caught <typeparamref name="TException3"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception4Handler">An optional function that maps a caught <typeparamref name="TException4"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result> AsResult(
        Func<Task> sourceDelegate,
        Func<TException1, Error>? exception1Handler = null,
        Func<TException2, Error>? exception2Handler = null,
        Func<TException3, Error>? exception3Handler = null,
        Func<TException4, Error>? exception4Handler = null)
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
        catch (TException1 ex)
        {
            exception1Handler ??= TryCatch.DefaultExceptionHandler;
            return Result.Fail(exception1Handler(ex));
        }
        catch (TException2 ex)
        {
            exception2Handler ??= TryCatch.DefaultExceptionHandler;
            return Result.Fail(exception2Handler(ex));
        }
        catch (TException3 ex)
        {
            exception3Handler ??= TryCatch.DefaultExceptionHandler;
            return Result.Fail(exception3Handler(ex));
        }
        catch (TException4 ex)
        {
            exception4Handler ??= TryCatch.DefaultExceptionHandler;
            return Result.Fail(exception4Handler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. If the delegate throws an exception of type <typeparamref name="TException1"/>,
    /// <typeparamref name="TException2"/>, <typeparamref name="TException3"/>, or <typeparamref name="TException4"/>, it is
    /// caught and used to create the error for the returned <c>Fail</c> result. Otherwise, if the delegate throws some other
    /// exception, it is not caught.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exception1Handler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception2Handler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception3Handler">An optional function that maps a caught <typeparamref name="TException3"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception4Handler">An optional function that maps a caught <typeparamref name="TException4"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result<T> AsResult<T>(
        Func<T?> sourceDelegate,
        Func<TException1, Error>? exception1Handler = null,
        Func<TException2, Error>? exception2Handler = null,
        Func<TException3, Error>? exception3Handler = null,
        Func<TException4, Error>? exception4Handler = null)
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
        catch (TException1 ex)
        {
            exception1Handler ??= TryCatch.DefaultExceptionHandler;
            return Result<T>.Fail(exception1Handler(ex));
        }
        catch (TException2 ex)
        {
            exception2Handler ??= TryCatch.DefaultExceptionHandler;
            return Result<T>.Fail(exception2Handler(ex));
        }
        catch (TException3 ex)
        {
            exception3Handler ??= TryCatch.DefaultExceptionHandler;
            return Result<T>.Fail(exception3Handler(ex));
        }
        catch (TException4 ex)
        {
            exception4Handler ??= TryCatch.DefaultExceptionHandler;
            return Result<T>.Fail(exception4Handler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. If the delegate throws an exception of type <typeparamref name="TException1"/>,
    /// <typeparamref name="TException2"/>, <typeparamref name="TException3"/>, or <typeparamref name="TException4"/>, it is
    /// caught and used to create the error for the returned <c>Fail</c> result. Otherwise, if the delegate throws some other
    /// exception, it is not caught.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exception1Handler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception2Handler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception3Handler">An optional function that maps a caught <typeparamref name="TException3"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception4Handler">An optional function that maps a caught <typeparamref name="TException4"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> AsResult<T>(
        Func<Task<T>> sourceDelegate,
        Func<TException1, Error>? exception1Handler = null,
        Func<TException2, Error>? exception2Handler = null,
        Func<TException3, Error>? exception3Handler = null,
        Func<TException4, Error>? exception4Handler = null)
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
        catch (TException1 ex)
        {
            exception1Handler ??= TryCatch.DefaultExceptionHandler;
            return Result<T>.Fail(exception1Handler(ex));
        }
        catch (TException2 ex)
        {
            exception2Handler ??= TryCatch.DefaultExceptionHandler;
            return Result<T>.Fail(exception2Handler(ex));
        }
        catch (TException3 ex)
        {
            exception3Handler ??= TryCatch.DefaultExceptionHandler;
            return Result<T>.Fail(exception3Handler(ex));
        }
        catch (TException4 ex)
        {
            exception4Handler ??= TryCatch.DefaultExceptionHandler;
            return Result<T>.Fail(exception4Handler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Maybe{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. If the delegate throws an exception of type <typeparamref name="TException1"/>,
    /// <typeparamref name="TException2"/>, <typeparamref name="TException3"/>, or <typeparamref name="TException4"/>, it is
    /// caught and used to create the error for the returned <c>Fail</c> result. Otherwise, if the delegate throws some other
    /// exception, it is not caught.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exception1Handler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception2Handler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception3Handler">An optional function that maps a caught <typeparamref name="TException3"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception4Handler">An optional function that maps a caught <typeparamref name="TException4"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Maybe<T> AsMaybe<T>(
        Func<T?> sourceDelegate,
        Func<TException1, Error>? exception1Handler = null,
        Func<TException2, Error>? exception2Handler = null,
        Func<TException3, Error>? exception3Handler = null,
        Func<TException4, Error>? exception4Handler = null)
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
        catch (TException1 ex)
        {
            exception1Handler ??= TryCatch.DefaultExceptionHandler;
            return Maybe<T>.Fail(exception1Handler(ex));
        }
        catch (TException2 ex)
        {
            exception2Handler ??= TryCatch.DefaultExceptionHandler;
            return Maybe<T>.Fail(exception2Handler(ex));
        }
        catch (TException3 ex)
        {
            exception3Handler ??= TryCatch.DefaultExceptionHandler;
            return Maybe<T>.Fail(exception3Handler(ex));
        }
        catch (TException4 ex)
        {
            exception4Handler ??= TryCatch.DefaultExceptionHandler;
            return Maybe<T>.Fail(exception4Handler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Maybe{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. If the delegate throws an exception of type <typeparamref name="TException1"/>,
    /// <typeparamref name="TException2"/>, <typeparamref name="TException3"/>, or <typeparamref name="TException4"/>, it is
    /// caught and used to create the error for the returned <c>Fail</c> result. Otherwise, if the delegate throws some other
    /// exception, it is not caught.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exception1Handler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception2Handler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception3Handler">An optional function that maps a caught <typeparamref name="TException3"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <param name="exception4Handler">An optional function that maps a caught <typeparamref name="TException4"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling the function from the
    ///     <see cref="TryCatch.DefaultExceptionHandler"/> property.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> AsMaybe<T>(
        Func<Task<T>> sourceDelegate,
        Func<TException1, Error>? exception1Handler = null,
        Func<TException2, Error>? exception2Handler = null,
        Func<TException3, Error>? exception3Handler = null,
        Func<TException4, Error>? exception4Handler = null)
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
        catch (TException1 ex)
        {
            exception1Handler ??= TryCatch.DefaultExceptionHandler;
            return Maybe<T>.Fail(exception1Handler(ex));
        }
        catch (TException2 ex)
        {
            exception2Handler ??= TryCatch.DefaultExceptionHandler;
            return Maybe<T>.Fail(exception2Handler(ex));
        }
        catch (TException3 ex)
        {
            exception3Handler ??= TryCatch.DefaultExceptionHandler;
            return Maybe<T>.Fail(exception3Handler(ex));
        }
        catch (TException4 ex)
        {
            exception4Handler ??= TryCatch.DefaultExceptionHandler;
            return Maybe<T>.Fail(exception4Handler(ex));
        }
    }
}
