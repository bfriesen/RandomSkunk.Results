using static RandomSkunk.Results.Exceptions;

namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for converting delegates into result values by evaluating them inside try/catch blocks.
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
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Result ToResult(
        this Action source,
        Func<Exception, Error>? getError = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        try
        {
            source();
            return Result.Create.Success();
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result.Create.Fail(getError.Evaluate(ex));
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
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If the source function returns <see langword="null"/>.</exception>
    public static Result<T> ToResult<T>(
        this Func<T> source,
        Func<Exception, Error>? getError = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        T value;
        try
        {
            value = source();
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Create.Fail(getError.Evaluate(ex));
        }

        if (value is null)
            throw FunctionMustNotReturnNull(nameof(source));

        return Result<T>.Create.Success(value);
    }

    /// <summary>
    /// Converts the specified <see cref="Func{TResult}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Some</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception, the exception is used to construct the error of the returned <c>Fail</c> result.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Maybe<T> ToMaybe<T>(
        this Func<T> source,
        Func<Exception, Error>? getError = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        T value;
        try
        {
            value = source();
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Create.Fail(getError.Evaluate(ex));
        }

        if (value is null)
            return Maybe<T>.Create.None();

        return Maybe<T>.Create.Some(value);
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncAction"/> delegate to a <see cref="Result"/> by evaluating it inside a try/catch
    /// block.
    /// <para>
    /// If the function doesn't throw an exception, a <c>Success</c> result is returned. If the function throws an exception, the
    /// exception is used to construct the error of the returned <c>Fail</c> result.
    /// </para>
    /// </summary>
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Result> ToResultAsync(
        this AsyncAction source,
        Func<Exception, Error>? getError = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        try
        {
            await source();
            return Result.Create.Success();
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result.Create.Fail(getError.Evaluate(ex));
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
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If the source function returns <see langword="null"/>.</exception>
    public static async Task<Result<T>> ToResultAsync<T>(
        this AsyncFunc<T> source,
        Func<Exception, Error>? getError = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        T value;
        try
        {
            value = await source();
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Create.Fail(getError.Evaluate(ex));
        }

        if (value is null)
            throw FunctionMustNotReturnNull(nameof(source));

        return Result<T>.Create.Success(value);
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncFunc{T}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Some</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception, the exception is used to construct the error of the returned <c>Fail</c> result.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> ToMaybeAsync<T>(
        this AsyncFunc<T> source,
        Func<Exception, Error>? getError = null,
        Func<Exception, bool>? exceptionPredicate = null)
    {
        T value;
        try
        {
            value = await source();
        }
        catch (Exception ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Create.Fail(getError.Evaluate(ex));
        }

        if (value is null)
            return Maybe<T>.Create.None();

        return Maybe<T>.Create.Some(value);
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
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Result ToResult<TException>(
        this Action source,
        Func<TException, Error>? getError = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        try
        {
            source();
            return Result.Create.Success();
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result.Create.Fail(getError.Evaluate(ex));
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
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If the source function returns <see langword="null"/>.</exception>
    public static Result<T> ToResult<T, TException>(
        this Func<T> source,
        Func<TException, Error>? getError = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        T value;
        try
        {
            value = source();
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Create.Fail(getError.Evaluate(ex));
        }

        if (value is null)
            throw FunctionMustNotReturnNull(nameof(source));

        return Result<T>.Create.Success(value);
    }

    /// <summary>
    /// Converts the specified <see cref="Func{TResult}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Some</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception of type <typeparamref name="TException"/>, the exception is used to construct the
    /// error of the returned <c>Fail</c> result. If the function throws some other type of exception, the exception is not
    /// caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException">The type of exception to catch.</typeparam>
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Maybe<T> ToMaybe<T, TException>(
        this Func<T> source,
        Func<TException, Error>? getError = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        T value;
        try
        {
            value = source();
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Create.Fail(getError.Evaluate(ex));
        }

        if (value is null)
            return Maybe<T>.Create.None();

        return Maybe<T>.Create.Some(value);
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
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Result> ToResultAsync<TException>(
        this AsyncAction source,
        Func<TException, Error>? getError = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        try
        {
            await source();
            return Result.Create.Success();
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result.Create.Fail(getError.Evaluate(ex));
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
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If the source function returns <see langword="null"/>.</exception>
    public static async Task<Result<T>> ToResultAsync<T, TException>(
        this AsyncFunc<T> source,
        Func<TException, Error>? getError = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        T value;
        try
        {
            value = await source();
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Result<T>.Create.Fail(getError.Evaluate(ex));
        }

        if (value is null)
            throw FunctionMustNotReturnNull(nameof(source));

        return Result<T>.Create.Success(value);
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncFunc{T}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Some</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception of type <typeparamref name="TException"/>, the exception is used to construct the
    /// error of the returned <c>Fail</c> result. If the function throws some other type of exception, the exception is not
    /// caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException">The type of exception to catch.</typeparam>
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError">
    /// An optional function that maps an exception to a <c>Fail</c> result's error. If <see langword="null"/>, the error is
    /// created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exceptionPredicate">
    /// An optional predicate function that is applied to the catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> ToMaybeAsync<T, TException>(
        this AsyncFunc<T> source,
        Func<TException, Error>? getError = null,
        Func<TException, bool>? exceptionPredicate = null)
        where TException : Exception
    {
        T value;
        try
        {
            value = await source();
        }
        catch (TException ex) when (exceptionPredicate.Evaluate(ex))
        {
            return Maybe<T>.Create.Fail(getError.Evaluate(ex));
        }

        if (value is null)
            return Maybe<T>.Create.None();

        return Maybe<T>.Create.Some(value);
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
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError1">
    /// An optional function that maps the first exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="getError2">
    /// An optional function that maps the second exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exception1Predicate">
    /// An optional predicate function that is applied to the first catch block's <c>when</c> clause.
    /// </param>
    /// <param name="exception2Predicate">
    /// An optional predicate function that is applied to the second catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Result ToResult<TException1, TException2>(
        this Action source,
        Func<TException1, Error>? getError1 = null,
        Func<TException2, Error>? getError2 = null,
        Func<TException1, bool>? exception1Predicate = null,
        Func<TException2, bool>? exception2Predicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        try
        {
            source();
            return Result.Create.Success();
        }
        catch (TException1 ex) when (exception1Predicate.Evaluate(ex))
        {
            return Result.Create.Fail(getError1.Evaluate(ex));
        }
        catch (TException2 ex) when (exception2Predicate.Evaluate(ex))
        {
            return Result.Create.Fail(getError2.Evaluate(ex));
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
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError1">
    /// An optional function that maps the first exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="getError2">
    /// An optional function that maps the second exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exception1Predicate">
    /// An optional predicate function that is applied to the first catch block's <c>when</c> clause.
    /// </param>
    /// <param name="exception2Predicate">
    /// An optional predicate function that is applied to the second catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If the source function returns <see langword="null"/>.</exception>
    public static Result<T> ToResult<T, TException1, TException2>(
        this Func<T> source,
        Func<TException1, Error>? getError1 = null,
        Func<TException2, Error>? getError2 = null,
        Func<TException1, bool>? exception1Predicate = null,
        Func<TException2, bool>? exception2Predicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        T value;
        try
        {
            value = source();
        }
        catch (TException1 ex) when (exception1Predicate.Evaluate(ex))
        {
            return Result<T>.Create.Fail(getError1.Evaluate(ex));
        }
        catch (TException2 ex) when (exception2Predicate.Evaluate(ex))
        {
            return Result<T>.Create.Fail(getError2.Evaluate(ex));
        }

        if (value is null)
            throw FunctionMustNotReturnNull(nameof(source));

        return Result<T>.Create.Success(value);
    }

    /// <summary>
    /// Converts the specified <see cref="Func{TResult}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Some</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception of type <typeparamref name="TException1"/> or <typeparamref name="TException2"/>, the
    /// exception is used to construct the error of the returned <c>Fail</c> result. If the function throws some other type of
    /// exception, the exception is not caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException1">The first type of exception to catch.</typeparam>
    /// <typeparam name="TException2">The second type of exception to catch.</typeparam>
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError1">
    /// An optional function that maps the first exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="getError2">
    /// An optional function that maps the second exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exception1Predicate">
    /// An optional predicate function that is applied to the first catch block's <c>when</c> clause.
    /// </param>
    /// <param name="exception2Predicate">
    /// An optional predicate function that is applied to the second catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Maybe<T> ToMaybe<T, TException1, TException2>(
        this Func<T> source,
        Func<TException1, Error>? getError1 = null,
        Func<TException2, Error>? getError2 = null,
        Func<TException1, bool>? exception1Predicate = null,
        Func<TException2, bool>? exception2Predicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        T value;
        try
        {
            value = source();
        }
        catch (TException1 ex) when (exception1Predicate.Evaluate(ex))
        {
            return Maybe<T>.Create.Fail(getError1.Evaluate(ex));
        }
        catch (TException2 ex) when (exception2Predicate.Evaluate(ex))
        {
            return Maybe<T>.Create.Fail(getError2.Evaluate(ex));
        }

        if (value is null)
            return Maybe<T>.Create.None();

        return Maybe<T>.Create.Some(value);
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncAction"/> delegate to a <see cref="Result"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception, a <c>Success</c> result is returned. If the function throws an exception of
    /// type <typeparamref name="TException1"/> or <typeparamref name="TException2"/>, the exception is used to construct the
    /// error of the returned <c>Fail</c> result. If the function throws some other type of exception, the exception is not
    /// caught.
    /// </para>
    /// </summary>
    /// <typeparam name="TException1">The first type of exception to catch.</typeparam>
    /// <typeparam name="TException2">The second type of exception to catch.</typeparam>
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError1">
    /// An optional function that maps the first exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="getError2">
    /// An optional function that maps the second exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exception1Predicate">
    /// An optional predicate function that is applied to the first catch block's <c>when</c> clause.
    /// </param>
    /// <param name="exception2Predicate">
    /// An optional predicate function that is applied to the second catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Result> ToResultAsync<TException1, TException2>(
        this AsyncAction source,
        Func<TException1, Error>? getError1 = null,
        Func<TException2, Error>? getError2 = null,
        Func<TException1, bool>? exception1Predicate = null,
        Func<TException2, bool>? exception2Predicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        try
        {
            await source();
            return Result.Create.Success();
        }
        catch (TException1 ex) when (exception1Predicate.Evaluate(ex))
        {
            return Result.Create.Fail(getError1.Evaluate(ex));
        }
        catch (TException2 ex) when (exception2Predicate.Evaluate(ex))
        {
            return Result.Create.Fail(getError2.Evaluate(ex));
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
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError1">
    /// An optional function that maps the first exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="getError2">
    /// An optional function that maps the second exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exception1Predicate">
    /// An optional predicate function that is applied to the first catch block's <c>when</c> clause.
    /// </param>
    /// <param name="exception2Predicate">
    /// An optional predicate function that is applied to the second catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If the source function returns <see langword="null"/>.</exception>
    public static async Task<Result<T>> ToResultAsync<T, TException1, TException2>(
        this AsyncFunc<T> source,
        Func<TException1, Error>? getError1 = null,
        Func<TException2, Error>? getError2 = null,
        Func<TException1, bool>? exception1Predicate = null,
        Func<TException2, bool>? exception2Predicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        T value;
        try
        {
            value = await source();
        }
        catch (TException1 ex) when (exception1Predicate.Evaluate(ex))
        {
            return Result<T>.Create.Fail(getError1.Evaluate(ex));
        }
        catch (TException2 ex) when (exception2Predicate.Evaluate(ex))
        {
            return Result<T>.Create.Fail(getError2.Evaluate(ex));
        }

        if (value is null)
            throw FunctionMustNotReturnNull(nameof(source));

        return Result<T>.Create.Success(value);
    }

    /// <summary>
    /// Converts the specified <see cref="AsyncFunc{T}"/> delegate to a <see cref="Maybe{T}"/> by evaluating it inside a
    /// try/catch block.
    /// <para>
    /// If the function doesn't throw an exception and doesn't return null, its return value is the value of the returned
    /// <c>Some</c> result. If the function doesn't throw an exception but does return null, a <c>None</c> result is returned.
    /// If the function throws an exception of type <typeparamref name="TException1"/> or <typeparamref name="TException2"/>, the
    /// exception is used to construct the error of the returned <c>Fail</c> result. If the function throws some other type of
    /// exception, the exception is not caught.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <typeparam name="TException1">The first type of exception to catch.</typeparam>
    /// <typeparam name="TException2">The second type of exception to catch.</typeparam>
    /// <param name="source">The delegate to convert.</param>
    /// <param name="getError1">
    /// An optional function that maps the first exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="getError2">
    /// An optional function that maps the second exception type to a <c>Fail</c> result's error. If <see langword="null"/>, the
    /// error is created by calling <see cref="Error.FromException"/>.
    /// </param>
    /// <param name="exception1Predicate">
    /// An optional predicate function that is applied to the first catch block's <c>when</c> clause.
    /// </param>
    /// <param name="exception2Predicate">
    /// An optional predicate function that is applied to the second catch block's <c>when</c> clause.
    /// </param>
    /// <returns>A result representing the outcome of evaluating the delegate.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> ToMaybeAsync<T, TException1, TException2>(
        this AsyncFunc<T> source,
        Func<TException1, Error>? getError1 = null,
        Func<TException2, Error>? getError2 = null,
        Func<TException1, bool>? exception1Predicate = null,
        Func<TException2, bool>? exception2Predicate = null)
        where TException1 : Exception
        where TException2 : Exception
    {
        T value;
        try
        {
            value = await source();
        }
        catch (TException1 ex) when (exception1Predicate.Evaluate(ex))
        {
            return Maybe<T>.Create.Fail(getError1.Evaluate(ex));
        }
        catch (TException2 ex) when (exception2Predicate.Evaluate(ex))
        {
            return Maybe<T>.Create.Fail(getError2.Evaluate(ex));
        }

        if (value is null)
            return Maybe<T>.Create.None();

        return Maybe<T>.Create.Some(value);
    }

    private static Error Evaluate<TException>(this Func<TException, Error>? getError, TException exception)
        where TException : Exception =>
        getError is null
            ? Error.FromException(exception)
            : getError(exception);

    private static bool Evaluate<TException>(this Func<TException, bool>? exceptionPredicate, TException exception)
        where TException : Exception =>
        exceptionPredicate is null || exceptionPredicate(exception);
}
