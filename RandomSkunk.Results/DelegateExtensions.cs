namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for converting delegates into result objects by evaluating them inside try/catch blocks.
/// </summary>
public static class DelegateExtensions
{
    /// <summary>
    /// Converts the specified <see cref="Action"/> delegate to a <see cref="Result"/> by evaluating it inside a try/catch block.
    /// <para>
    /// If the function doesn't throw an exception, a <c>Success</c> result is returned. If the function throws an exception, the
    /// exception is used to construct the error of the returned <c>Fail</c> result.
    /// </para>
    /// </summary>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result TryInvokeAsResult(
        this Action sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        try
        {
            sourceDelegate();
            return Result.Success();
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result.Fail(exceptionHandler.Evaluate(ex));
        }
    }

    /// <summary>
    /// Converts the specified <see cref="Func{TResult}"/> delegate to a <see cref="Result{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception, its return value is the value of the returned <c>Success</c> result. If the
    /// function throws an exception, the exception is used to construct the error of the returned <c>Fail</c> result.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result<T> TryInvokeAsResult<T>(
        this Func<T> sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        if (sourceDelegate is null) throw new ArgumentNullException(nameof(sourceDelegate));

        T value;
        try
        {
            value = sourceDelegate();
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Fail(exceptionHandler.Evaluate(ex));
        }

        return value.ToResult();
    }

    /// <summary>
    /// Converts the specified <see cref="Func{TResult}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Success</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception, the exception is used to construct the error of the returned <c>Fail</c> result.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Maybe<T> TryInvokeAsMaybe<T>(
        this Func<T> sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        T value;
        try
        {
            value = sourceDelegate();
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Fail(exceptionHandler.Evaluate(ex));
        }

        return value.ToMaybe();
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncAction"/> delegate to a <see cref="Result"/> by evaluating it inside a try/catch
    /// block.
    /// <para>
    /// If the function doesn't throw an exception, a <c>Success</c> result is returned. If the function throws an exception, the
    /// exception is used to construct the error of the returned <c>Fail</c> result.
    /// </para>
    /// </summary>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result> TryInvokeAsResultAsync(
        this AsyncAction sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        try
        {
            await sourceDelegate().ConfigureAwait(false);
            return Result.Success();
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result.Fail(exceptionHandler.Evaluate(ex));
        }
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncFunc{T}"/> delegate to a <see cref="Result{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception, its return value is the value of the returned <c>Success</c> result. If the
    /// function throws an exception, the exception is used to construct the error of the returned <c>Fail</c> result.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> TryInvokeAsResultAsync<T>(
        this AsyncFunc<T> sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        T value;
        try
        {
            value = await sourceDelegate().ConfigureAwait(false);
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Fail(exceptionHandler.Evaluate(ex));
        }

        return value.ToResult();
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncFunc{T}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Success</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception, the exception is used to construct the error of the returned <c>Fail</c> result.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <see cref="Exception"/> to a <c>Fail</c> result's
    ///     error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> TryInvokeAsMaybeAsync<T>(
        this AsyncFunc<T> sourceDelegate,
        Func<Exception, Error>? exceptionHandler = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        T value;
        try
        {
            value = await sourceDelegate().ConfigureAwait(false);
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Fail(exceptionHandler.Evaluate(ex));
        }

        return value.ToMaybe();
    }

    /// <summary>
    /// Converts the specified <see cref="Action"/> delegate to a <see cref="Result"/> by evaluating it inside a try/catch block.
    /// <para>
    /// If the function doesn't throw an exception, a <c>Success</c> result is returned. If the function throws an exception of
    /// type <typeparamref name="TException"/>, the exception is used to construct the error of the returned <c>Fail</c> result.
    /// If the function throws some other type of exception, the exception is not caught.
    /// </para>
    /// </summary>
    /// <typeparam name="TException">The type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result TryInvokeAsResult<TException>(
        this Action sourceDelegate,
        Func<TException, Error>? exceptionHandler = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        try
        {
            sourceDelegate();
            return Result.Success();
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result.Fail(exceptionHandler.Evaluate(ex));
        }
    }

    /// <summary>
    /// Converts the specified <see cref="Func{TResult}"/> delegate to a <see cref="Result{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception, its return value is the value of the returned <c>Success</c> result. If the
    /// function throws an exception of type <typeparamref name="TException"/>, the exception is used to construct the error of
    /// the returned <c>Fail</c> result. If the function throws some other type of exception, the exception is not caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException">The type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result<T> TryInvokeAsResult<T, TException>(
        this Func<T> sourceDelegate,
        Func<TException, Error>? exceptionHandler = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        T value;
        try
        {
            value = sourceDelegate();
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Fail(exceptionHandler.Evaluate(ex));
        }

        return value.ToResult();
    }

    /// <summary>
    /// Converts the specified <see cref="Func{TResult}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Success</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception of type <typeparamref name="TException"/>, the exception is used to construct the
    /// error of the returned <c>Fail</c> result. If the function throws some other type of exception, the exception is not
    /// caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException">The type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Maybe<T> TryInvokeAsMaybe<T, TException>(
        this Func<T> sourceDelegate,
        Func<TException, Error>? exceptionHandler = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        T value;
        try
        {
            value = sourceDelegate();
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Fail(exceptionHandler.Evaluate(ex));
        }

        return value.ToMaybe();
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncAction"/> delegate to a <see cref="Result"/> by evaluating it inside a try/catch
    /// block.
    /// <para>
    /// If the function doesn't throw an exception, a <c>Success</c> result is returned. If the function throws an exception of
    /// type <typeparamref name="TException"/>, the exception is used to construct the error of the returned <c>Fail</c> result.
    /// If the function throws some other type of exception, the exception is not caught.
    /// </para>
    /// </summary>
    /// <typeparam name="TException">The type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result> TryInvokeAsResultAsync<TException>(
        this AsyncAction sourceDelegate,
        Func<TException, Error>? exceptionHandler = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        try
        {
            await sourceDelegate().ConfigureAwait(false);
            return Result.Success();
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result.Fail(exceptionHandler.Evaluate(ex));
        }
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncFunc{T}"/> delegate to a <see cref="Result{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception, its return value is the value of the returned <c>Success</c> result. If the
    /// function throws an exception of type <typeparamref name="TException"/>, the exception is used to construct the error of
    /// the returned <c>Fail</c> result. If the function throws some other type of exception, the exception is not caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException">The type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> TryInvokeAsResultAsync<T, TException>(
        this AsyncFunc<T> sourceDelegate,
        Func<TException, Error>? exceptionHandler = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        T value;
        try
        {
            value = await sourceDelegate().ConfigureAwait(false);
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Fail(exceptionHandler.Evaluate(ex));
        }

        return value.ToResult();
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncFunc{T}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Success</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception of type <typeparamref name="TException"/>, the exception is used to construct the
    /// error of the returned <c>Fail</c> result. If the function throws some other type of exception, the exception is not
    /// caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException">The type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="exceptionHandler">An optional function that maps a caught <typeparamref name="TException"/> to a <c>Fail</c>
    ///     result's error. If <see langword="null"/>, the error is created by calling <see cref="Error.FromException"/>.</param>
    /// <param name="exceptionPredicate">An optional predicate function that is applied to the catch block's <c>when</c> clause.
    ///     </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> TryInvokeAsMaybeAsync<T, TException>(
        this AsyncFunc<T> sourceDelegate,
        Func<TException, Error>? exceptionHandler = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        T value;
        try
        {
            value = await sourceDelegate().ConfigureAwait(false);
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Fail(exceptionHandler.Evaluate(ex));
        }

        return value.ToMaybe();
    }

    /// <summary>
    /// Converts the specified <see cref="Action"/> delegate to a <see cref="Result"/> by evaluating it inside a try/catch block.
    /// <para>
    /// If the function doesn't throw an exception, a <c>Success</c> result is returned. If the function throws an exception of
    /// type <typeparamref name="TException1"/> or <typeparamref name="TException2"/>, the exception is used to construct the
    /// error of the returned <c>Fail</c> result. If the function throws some other type of exception, the exception is not
    /// caught.
    /// </para>
    /// </summary>
    /// <typeparam name="TException1">The first type of exception to catch.</typeparam>
    /// <typeparam name="TException2">The second type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="firstExceptionHandler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="secondExceptionHandler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="firstExceptionPredicate">An optional predicate function that is applied to the first catch block's
    ///     <c>when</c> clause.</param>
    /// <param name="secondExceptionPredicate">An optional predicate function that is applied to the second catch block's
    ///     <c>when</c> clause.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result TryInvokeAsResult<TException1, TException2>(
        this Action sourceDelegate,
        Func<TException1, Error>? firstExceptionHandler = null,
        Func<TException2, Error>? secondExceptionHandler = null,
        Func<TException1, bool>? firstExceptionPredicate = null,
        Func<TException2, bool>? secondExceptionPredicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        try
        {
            sourceDelegate();
            return Result.Success();
        }
        catch (TException1 ex) when (firstExceptionPredicate.Evaluate(ex))
        {
            return Result.Fail(firstExceptionHandler.Evaluate(ex));
        }
        catch (TException2 ex) when (secondExceptionPredicate.Evaluate(ex))
        {
            return Result.Fail(secondExceptionHandler.Evaluate(ex));
        }
    }

    /// <summary>
    /// Converts the specified <see cref="Func{TResult}"/> delegate to a <see cref="Result{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception, its return value is the value of the returned <c>Success</c> result. If the
    /// function throws an exception of type <typeparamref name="TException1"/> or <typeparamref name="TException2"/>, the
    /// exception is used to construct the error of the returned <c>Fail</c> result. If the function throws some other type of
    /// exception, the exception is not caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException1">The first type of exception to catch.</typeparam>
    /// <typeparam name="TException2">The second type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="firstExceptionHandler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="secondExceptionHandler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="firstExceptionPredicate">An optional predicate function that is applied to the first catch block's
    ///     <c>when</c> clause.</param>
    /// <param name="secondExceptionPredicate">An optional predicate function that is applied to the second catch block's
    ///     <c>when</c> clause.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Result<T> TryInvokeAsResult<T, TException1, TException2>(
        this Func<T> sourceDelegate,
        Func<TException1, Error>? firstExceptionHandler = null,
        Func<TException2, Error>? secondExceptionHandler = null,
        Func<TException1, bool>? firstExceptionPredicate = null,
        Func<TException2, bool>? secondExceptionPredicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        T value;
        try
        {
            value = sourceDelegate();
        }
        catch (TException1 ex) when (firstExceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Fail(firstExceptionHandler.Evaluate(ex));
        }
        catch (TException2 ex) when (secondExceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Fail(secondExceptionHandler.Evaluate(ex));
        }

        return value.ToResult();
    }

    /// <summary>
    /// Converts the specified <see cref="Func{TResult}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Success</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception of type <typeparamref name="TException1"/> or <typeparamref name="TException2"/>, the
    /// exception is used to construct the error of the returned <c>Fail</c> result. If the function throws some other type of
    /// exception, the exception is not caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException1">The first type of exception to catch.</typeparam>
    /// <typeparam name="TException2">The second type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="firstExceptionHandler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="secondExceptionHandler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="firstExceptionPredicate">An optional predicate function that is applied to the first catch block's
    ///     <c>when</c> clause.</param>
    /// <param name="secondExceptionPredicate">An optional predicate function that is applied to the second catch block's
    ///     <c>when</c> clause.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static Maybe<T> TryInvokeAsMaybe<T, TException1, TException2>(
        this Func<T> sourceDelegate,
        Func<TException1, Error>? firstExceptionHandler = null,
        Func<TException2, Error>? secondExceptionHandler = null,
        Func<TException1, bool>? firstExceptionPredicate = null,
        Func<TException2, bool>? secondExceptionPredicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        T value;
        try
        {
            value = sourceDelegate();
        }
        catch (TException1 ex) when (firstExceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Fail(firstExceptionHandler.Evaluate(ex));
        }
        catch (TException2 ex) when (secondExceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Fail(secondExceptionHandler.Evaluate(ex));
        }

        return value.ToMaybe();
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncAction"/> delegate to a <see cref="Result"/> by evaluating it inside a try/catch
    /// block.
    /// <para>
    /// If the function doesn't throw an exception, a <c>Success</c> result is returned. If the function throws an exception of
    /// type <typeparamref name="TException1"/> or <typeparamref name="TException2"/>, the exception is used to construct the
    /// error of the returned <c>Fail</c> result. If the function throws some other type of exception, the exception is not
    /// caught.
    /// </para>
    /// </summary>
    /// <typeparam name="TException1">The first type of exception to catch.</typeparam>
    /// <typeparam name="TException2">The second type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="firstExceptionHandler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="secondExceptionHandler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="firstExceptionPredicate">An optional predicate function that is applied to the first catch block's
    ///     <c>when</c> clause.</param>
    /// <param name="secondExceptionPredicate">An optional predicate function that is applied to the second catch block's
    ///     <c>when</c> clause.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result> TryInvokeAsResultAsync<TException1, TException2>(
        this AsyncAction sourceDelegate,
        Func<TException1, Error>? firstExceptionHandler = null,
        Func<TException2, Error>? secondExceptionHandler = null,
        Func<TException1, bool>? firstExceptionPredicate = null,
        Func<TException2, bool>? secondExceptionPredicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        try
        {
            await sourceDelegate().ConfigureAwait(false);
            return Result.Success();
        }
        catch (TException1 ex) when (firstExceptionPredicate.Evaluate(ex))
        {
            return Result.Fail(firstExceptionHandler.Evaluate(ex));
        }
        catch (TException2 ex) when (secondExceptionPredicate.Evaluate(ex))
        {
            return Result.Fail(secondExceptionHandler.Evaluate(ex));
        }
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncFunc{T}"/> delegate to a <see cref="Result{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception, its return value is the value of the returned <c>Success</c> result. If the
    /// function throws an exception of type <typeparamref name="TException1"/> or <typeparamref name="TException2"/>, the
    /// exception is used to construct the error of the returned <c>Fail</c> result. If the function throws some other type of
    /// exception, the exception is not caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException1">The first type of exception to catch.</typeparam>
    /// <typeparam name="TException2">The second type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="firstExceptionHandler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="secondExceptionHandler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="firstExceptionPredicate">An optional predicate function that is applied to the first catch block's
    ///     <c>when</c> clause.</param>
    /// <param name="secondExceptionPredicate">An optional predicate function that is applied to the second catch block's
    ///     <c>when</c> clause.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> TryInvokeAsResultAsync<T, TException1, TException2>(
        this AsyncFunc<T> sourceDelegate,
        Func<TException1, Error>? firstExceptionHandler = null,
        Func<TException2, Error>? secondExceptionHandler = null,
        Func<TException1, bool>? firstExceptionPredicate = null,
        Func<TException2, bool>? secondExceptionPredicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        T value;
        try
        {
            value = await sourceDelegate().ConfigureAwait(false);
        }
        catch (TException1 ex) when (firstExceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Fail(firstExceptionHandler.Evaluate(ex));
        }
        catch (TException2 ex) when (secondExceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Fail(secondExceptionHandler.Evaluate(ex));
        }

        return value.ToResult();
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncFunc{T}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Success</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception of type <typeparamref name="TException1"/> or <typeparamref name="TException2"/>, the
    /// exception is used to construct the error of the returned <c>Fail</c> result. If the function throws some other type of
    /// exception, the exception is not caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException1">The first type of exception to catch.</typeparam>
    /// <typeparam name="TException2">The second type of exception to catch.</typeparam>
    /// <param name="sourceDelegate">The delegate to convert.</param>
    /// <param name="firstExceptionHandler">An optional function that maps a caught <typeparamref name="TException1"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="secondExceptionHandler">An optional function that maps a caught <typeparamref name="TException2"/> to a
    ///     <c>Fail</c> result's error. If <see langword="null"/>, the error is created by calling
    ///     <see cref="Error.FromException"/>.</param>
    /// <param name="firstExceptionPredicate">An optional predicate function that is applied to the first catch block's
    ///     <c>when</c> clause.</param>
    /// <param name="secondExceptionPredicate">An optional predicate function that is applied to the second catch block's
    ///     <c>when</c> clause.</param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceDelegate"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> TryInvokeAsMaybeAsync<T, TException1, TException2>(
        this AsyncFunc<T> sourceDelegate,
        Func<TException1, Error>? firstExceptionHandler = null,
        Func<TException2, Error>? secondExceptionHandler = null,
        Func<TException1, bool>? firstExceptionPredicate = null,
        Func<TException2, bool>? secondExceptionPredicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        T value;
        try
        {
            value = await sourceDelegate().ConfigureAwait(false);
        }
        catch (TException1 ex) when (firstExceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Fail(firstExceptionHandler.Evaluate(ex));
        }
        catch (TException2 ex) when (secondExceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Fail(secondExceptionHandler.Evaluate(ex));
        }

        return value.ToMaybe();
    }

    private static Error Evaluate<TException>(this Func<TException, Error>? exceptionHandler, TException exception)
        where TException : Exception =>
        exceptionHandler is null
            ? Error.FromException(exception)
            : exceptionHandler(exception);

    private static bool Evaluate<TException>(this Func<TException, bool>? exceptionPredicate, TException exception)
        where TException : Exception =>
        exceptionPredicate is null || exceptionPredicate(exception);
}
