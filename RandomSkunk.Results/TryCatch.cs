namespace RandomSkunk.Results;

/// <summary>
/// Defines methods that evaluate a delegate using a try-catch statement with a single catch block with exception of type
/// <see cref="Exception"/>.
/// </summary>
public static class TryCatch
{
    private static readonly Func<Exception, Error> _defaultExceptionHandler = GetErrorFromException;

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, a <c>Success</c> result is returned. Otherwise, if
    /// the delegate throws an exception, it is caught and used to create the error for the returned <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result AsResult(
        Action sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        exceptionHandler ??= _defaultExceptionHandler;

        try
        {
            sourceDelegate();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(exceptionHandler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, a <c>Success</c> result is returned. Otherwise, if
    /// the delegate throws an exception, it is caught and used to create the error for the returned <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result> AsResult(
        Func<Task> sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        exceptionHandler ??= _defaultExceptionHandler;

        try
        {
            await sourceDelegate().ConfigureAwait(false);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(exceptionHandler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. Otherwise, if the delegate throws an exception, it is caught and used to create the error for the
    /// returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result<T> AsResult<T>(
        Func<T?> sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        exceptionHandler ??= _defaultExceptionHandler;

        try
        {
            var value = sourceDelegate();
            return Result<T>.FromValue(value);
        }
        catch (Exception ex)
        {
            return Result<T>.Fail(exceptionHandler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. Otherwise, if the delegate throws an exception, it is caught and used to create the error for the
    /// returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> AsResult<T>(
        Func<Task<T>> sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        exceptionHandler ??= _defaultExceptionHandler;

        try
        {
            var value = await sourceDelegate().ConfigureAwait(false);
            return Result<T>.FromValue(value);
        }
        catch (Exception ex)
        {
            return Result<T>.Fail(exceptionHandler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Maybe{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. Otherwise, if the delegate throws an exception, it is caught and used to create the error for the
    /// returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Maybe<T> AsMaybe<T>(
        Func<T?> sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        exceptionHandler ??= _defaultExceptionHandler;

        try
        {
            var value = sourceDelegate();
            return Maybe<T>.FromValue(value);
        }
        catch (Exception ex)
        {
            return Maybe<T>.Fail(exceptionHandler(ex));
        }
    }

    /// <summary>
    /// Evaluates <paramref name="sourceDelegate"/> with a try-catch statement and returns a <see cref="Result{T}"/> representing
    /// the outcome of the operation. If the delegate evaluates successfully, its return value is used for the value of the
    /// returned result. Otherwise, if the delegate throws an exception, it is caught and used to create the error for the
    /// returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of <paramref name="sourceDelegate"/> and the type of the returned result value.
    ///     </typeparam>
    /// <param name="sourceDelegate">The delegate to evaluate.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> AsMaybe<T>(
        Func<Task<T>> sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        exceptionHandler ??= _defaultExceptionHandler;

        try
        {
            var value = await sourceDelegate().ConfigureAwait(false);
            return Maybe<T>.FromValue(value);
        }
        catch (Exception ex)
        {
            return Maybe<T>.Fail(exceptionHandler(ex));
        }
    }

    [StackTraceHidden]
    internal static Error GetErrorFromException(Exception ex) => Error.FromException(ex);
}
