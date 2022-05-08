using static RandomSkunk.Results.MaybeType;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The return type of the match method.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static T Match<T>(
        this Result source,
        Func<T> success,
        Func<Error, T> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.IsSuccess
            ? success()
            : fail(source.Error);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static void Match(
        this Result source,
        Action success,
        Action<Error> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        if (source.IsSuccess)
            success();
        else
            fail(source.Error);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The return type of the match method.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static Task<T> MatchAsync<T>(
        this Result source,
        Func<CancellationToken, Task<T>> success,
        Func<Error, CancellationToken, Task<T>> fail,
        CancellationToken cancellationToken = default)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.IsSuccess
            ? success(cancellationToken)
            : fail(source.Error, cancellationToken);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The return type of the match method.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static Task<T> MatchAsync<T>(
        this Result source,
        Func<Task<T>> success,
        Func<Error, Task<T>> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.MatchAsync(
            _ => success(),
            (error, _) => fail(error));
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static Task MatchAsync(
        this Result source,
        Func<CancellationToken, Task> success,
        Func<Error, CancellationToken, Task> fail,
        CancellationToken cancellationToken = default)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.IsSuccess
            ? success(cancellationToken)
            : fail(source.Error, cancellationToken);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static Task MatchAsync(
        this Result source,
        Func<Task> success,
        Func<Error, Task> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.MatchAsync(
            _ => success(),
            (error, _) => fail(error));
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>. The value of the <c>Success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error code
    /// of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static TReturn Match<T, TReturn>(
        this Result<T> source,
        Func<T, TReturn> success,
        Func<Error, TReturn> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.IsSuccess
            ? success(source.Value)
            : fail(source.Error);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>. The value of the <c>Success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error code
    /// of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static void Match<T>(
        this Result<T> source,
        Action<T> success,
        Action<Error> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        if (source.IsSuccess)
            success(source.Value);
        else
            fail(source.Error);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>. The value of the <c>Success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error code
    /// of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAsync<T, TReturn>(
        this Result<T> source,
        Func<T, CancellationToken, Task<TReturn>> success,
        Func<Error, CancellationToken, Task<TReturn>> fail,
        CancellationToken cancellationToken = default)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.IsSuccess
            ? success(source.Value, cancellationToken)
            : fail(source.Error, cancellationToken);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>. The value of the <c>Success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error code
    /// of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAsync<T, TReturn>(
        this Result<T> source,
        Func<T, Task<TReturn>> success,
        Func<Error, Task<TReturn>> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.MatchAsync(
            (value, _) => success(value),
            (error, _) => fail(error));
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>. The value of the <c>Success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error code
    /// of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static Task MatchAsync<T>(
        this Result<T> source,
        Func<T, CancellationToken, Task> success,
        Func<Error, CancellationToken, Task> fail,
        CancellationToken cancellationToken = default)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.IsSuccess
            ? success(source.Value, cancellationToken)
            : fail(source.Error, cancellationToken);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>. The value of the <c>Success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error code
    /// of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static Task MatchAsync<T>(
        this Result<T> source,
        Func<T, Task> success,
        Func<Error, Task> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.MatchAsync(
            (value, _) => success(value),
            (error, _) => fail(error));
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn Match<T, TReturn>(
        this Maybe<T> source,
        Func<T, TReturn> some,
        Func<TReturn> none,
        Func<Error, TReturn> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.Type switch
        {
            Some => some(source.Value),
            None => none(),
            _ => fail(source.Error),
        };
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public static void Match<T>(
        this Maybe<T> source,
        Action<T> some,
        Action none,
        Action<Error> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        switch (source.Type)
        {
            case Some: some(source.Value); break;
            case None: none(); break;
            default: fail(source.Error); break;
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, CancellationToken, Task<TReturn>> some,
        Func<CancellationToken, Task<TReturn>> none,
        Func<Error, CancellationToken, Task<TReturn>> fail,
        CancellationToken cancellationToken = default)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.Type switch
        {
            Some => some(source.Value, cancellationToken),
            None => none(cancellationToken),
            _ => fail(source.Error, cancellationToken),
        };
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, Task<TReturn>> some,
        Func<Task<TReturn>> none,
        Func<Error, Task<TReturn>> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.MatchAsync(
            (value, _) => some(value),
            _ => none(),
            (error, _) => fail(error));
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public static Task MatchAsync<T>(
        this Maybe<T> source,
        Func<T, CancellationToken, Task> some,
        Func<CancellationToken, Task> none,
        Func<Error, CancellationToken, Task> fail,
        CancellationToken cancellationToken = default)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.Type switch
        {
            Some => some(source.Value, cancellationToken),
            None => none(cancellationToken),
            _ => fail(source.Error, cancellationToken),
        };
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public static Task MatchAsync<T>(
        this Maybe<T> source,
        Func<T, Task> some,
        Func<Task> none,
        Func<Error, Task> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return source.MatchAsync(
            (value, _) => some(value),
            _ => none(),
            (error, _) => fail(error));
    }

    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="otherValue">The value to compare.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> used to determine equality of the values.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Success</c> result and its value equals
    /// <paramref name="otherValue"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(this Result<T> source, T otherValue, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        return source.IsSuccess && comparer.Equals(source.Value, otherValue);
    }

    /// <summary>
    /// Determines whether the value of the result is equal to another value as defined by the
    /// <paramref name="isSuccessValue"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="isSuccessValue">
    /// A function that defines the equality of the result value.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Success</c> result and
    /// <paramref name="isSuccessValue"/> evaluates to <see langword="true"/> when passed
    /// its value; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="isSuccessValue"/> is <see langword="null"/>.
    /// </exception>
    public static bool Equals<T>(this Result<T> source, Func<T, bool> isSuccessValue)
    {
        if (isSuccessValue is null) throw new ArgumentNullException(nameof(isSuccessValue));

        return source.IsSuccess && isSuccessValue(source.Value);
    }

    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="otherValue">The value to compare.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> used to determine equality of the values. If
    /// <see langword="null"/>, then <see cref="EqualityComparer{T}.Default"/> is used instead.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Some</c> result and its value equals
    /// <paramref name="otherValue"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(this Maybe<T> source, T otherValue, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        return source.IsSome && comparer.Equals(source.Value, otherValue);
    }

    /// <summary>
    /// Determines whether the value of the result is equal to another value as defined by the
    /// <paramref name="isSomeValue"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="isSomeValue">
    /// A function that defines the equality of the result value.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Some</c> result and
    /// <paramref name="isSomeValue"/> evaluates to <see langword="true"/> when passed
    /// its value; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="isSomeValue"/> is <see langword="null"/>.
    /// </exception>
    public static bool Equals<T>(this Maybe<T> source, Func<T, bool> isSomeValue)
    {
        if (isSomeValue is null) throw new ArgumentNullException(nameof(isSomeValue));

        return source.IsSome && isSomeValue(source.Value);
    }

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">
    /// The fallback value to return if this is not a <c>Success</c> result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Success</c> result; otherwise,
    /// <paramref name="fallbackValue"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    [return: NotNull]
    public static T GetValueOr<T>(this Result<T> source, [DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return source.IsSuccess ? source.Value : fallbackValue;
    }

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that creates the fallback value to return if this is not a <c>Success</c>
    /// result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Success</c> result; otherwise, the value returned
    /// by the <paramref name="getFallbackValue"/> function.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    [return: NotNull]
    public static T GetValueOr<T>(this Result<T> source, Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return source.IsSuccess ? source.Value : getFallbackValue()
            ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));
    }

    /// <summary>
    /// Gets the value of the <c>Some</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">
    /// The fallback value to return if this is not a <c>Some</c> result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Some</c> result; otherwise,
    /// <paramref name="fallbackValue"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    [return: NotNull]
    public static T GetValueOr<T>(this Maybe<T> source, [DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return source.IsSome ? source.Value : fallbackValue;
    }

    /// <summary>
    /// Gets the value of the <c>Some</c> result, or the specified fallback value if
    /// it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that creates the fallback value to return if this is not a <c>Some</c>
    /// result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>Some</c> result; otherwise, the value returned
    /// by the <paramref name="getFallbackValue"/> function.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    [return: NotNull]
    public static T GetValueOr<T>(this Maybe<T> source, Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return source.IsSome ? source.Value : getFallbackValue()
            ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result; otherwise, returns a
    /// new <c>Success</c> result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Or<T>(this Result<T> source, [DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return source.IsSuccess ? source : Result<T>.Create.Success(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result; otherwise, returns a
    /// new <c>Success</c> result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that returns the fallback value if the result is not <c>Success</c>.
    /// </param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<T> Or<T>(this Result<T> source, Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        if (source.IsSuccess)
            return source;

        var fallbackValue = getFallbackValue()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));

        return Result<T>.Create.Success(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Some</c> result; otherwise, returns a new
    /// <c>Some</c> result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>Some</c>.</param>
    /// <returns>A <c>Some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> Or<T>(this Maybe<T> source, [DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return source.IsSome ? source : Maybe<T>.Create.Some(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Some</c> result; otherwise, returns a new
    /// <c>Some</c> result with its value from evaluating the <paramref name="getFallbackValue"/>
    /// function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that returns the fallback value if the result is not <c>Some</c>.
    /// </param>
    /// <returns>A <c>Some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Maybe<T> Or<T>(this Maybe<T> source, Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        if (source.IsSome)
            return source;

        var fallbackValue = getFallbackValue()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));

        return Maybe<T>.Create.Some(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="source"/> or <paramref name="fallbackResult"/>.</returns>
    public static Result Else(this Result source, Result fallbackResult)
    {
        return source.IsSuccess ? source : fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result, else returns the result
    /// from evaluating the <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackResult">
    /// A function that returns the fallback result if the result is not <c>Success</c>.
    /// </param>
    /// <returns>
    /// Either <paramref name="source"/> or the value returned from
    /// <paramref name="getFallbackResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackResult"/> is <see langword="null"/>.
    /// </exception>
    public static Result Else(this Result source, Func<Result> getFallbackResult)
    {
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        return source.IsSuccess ? source : getFallbackResult();
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="source"/> or <paramref name="fallbackResult"/>.</returns>
    public static Result<T> Else<T>(this Result<T> source, Result<T> fallbackResult)
    {
        return source.IsSuccess ? source : fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result, else returns the result
    /// from evaluating the <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackResult">
    /// A function that returns the fallback result if the result is not <c>Success</c>.
    /// </param>
    /// <returns>
    /// Either <paramref name="source"/> or the value returned from
    /// <paramref name="getFallbackResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackResult"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Else<T>(this Result<T> source, Func<Result<T>> getFallbackResult)
    {
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        return source.IsSuccess ? source : getFallbackResult();
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Some</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Some</c>.</param>
    /// <returns>Either <paramref name="source"/> or <paramref name="fallbackResult"/>.</returns>
    public static Maybe<T> Else<T>(this Maybe<T> source, Maybe<T> fallbackResult)
    {
        return source.IsSome ? source : fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Some</c> result, else returns the result
    /// from evaluating the <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackResult">
    /// A function that returns the fallback result if the result is not <c>Some</c>.
    /// </param>
    /// <returns>
    /// Either <paramref name="source"/> or the value returned from
    /// <paramref name="getFallbackResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackResult"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> Else<T>(this Maybe<T> source, Func<Maybe<T>> getFallbackResult)
    {
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        return source.IsSome ? source : getFallbackResult();
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="map"/>
    /// function. The map function is only evaluated if the source is a <c>Success</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="map">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="map"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="map"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<TReturn> Map<T, TReturn>(this Result<T> source, Func<T, TReturn> map)
    {
        if (map is null) throw new ArgumentNullException(nameof(map));

        return source.Type switch
        {
            Success => Result<TReturn>.Create.Success(map(source.Value)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(map))),
            _ => Result<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>Success</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="mapAsync"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<Result<TReturn>> MapAsync<T, TReturn>(
        this Result<T> source,
        Func<T, CancellationToken, Task<TReturn>> mapAsync,
        CancellationToken cancellationToken = default)
    {
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        return source.Type switch
        {
            Success => Result<TReturn>.Create.Success((await mapAsync(source.Value, cancellationToken))
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(mapAsync))),
            _ => Result<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>Success</c> result, and
    /// the <see cref="Result{T}.Type"/> the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If if <paramref name="mapAsync"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<Result<TReturn>> MapAsync<T, TReturn>(
        this Result<T> source,
        Func<T, Task<TReturn>> mapAsync)
    {
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        return source.MapAsync((value, _) => mapAsync(value), default);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="map"/>
    /// function. The map function is only evaluated if the source is a <c>Some</c> result, and
    /// the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="map">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="map"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="map"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Maybe<TReturn> Map<T, TReturn>(this Maybe<T> source, Func<T, TReturn> map)
    {
        if (map is null) throw new ArgumentNullException(nameof(map));

        return source.Type switch
        {
            Some => Maybe<TReturn>.Create.Some(map(source.Value)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(map))),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>Some</c> result, and
    /// the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="mapAsync"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<Maybe<TReturn>> MapAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, CancellationToken, Task<TReturn>> mapAsync,
        CancellationToken cancellationToken = default)
    {
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        return source.Type switch
        {
            Some => Maybe<TReturn>.Create.Some((await mapAsync(source.Value, cancellationToken))
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(mapAsync))),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>Some</c> result, and
    /// the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="mapAsync"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<Maybe<TReturn>> MapAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, Task<TReturn>> mapAsync)
    {
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        return source.MapAsync((value, _) => mapAsync(value), default);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMap"/> function. The flat map function is only evaluated if the source
    /// is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is propagated
    /// to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMap">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMap"/> is <see langword="null"/>.
    /// </exception>
    public static Result<TReturn> FlatMap<T, TReturn>(this Result<T> source, Func<T, Result<TReturn>> flatMap)
    {
        if (flatMap is null) throw new ArgumentNullException(nameof(flatMap));

        return source.Type switch
        {
            Success => flatMap(source.Value),
            _ => Result<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result<TReturn>> FlatMapAsync<T, TReturn>(
        this Result<T> source,
        Func<T, CancellationToken, Task<Result<TReturn>>> flatMapAsync,
        CancellationToken cancellationToken = default)
    {
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        return source.Type switch
        {
            Success => await flatMapAsync(source.Value, cancellationToken),
            _ => Result<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static Task<Result<TReturn>> FlatMapAsync<T, TReturn>(
        this Result<T> source,
        Func<T, Task<Result<TReturn>>> flatMapAsync)
    {
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        return source.FlatMapAsync((value, _) => flatMapAsync(value), default);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMap"/> function. The flat map function is only evaluated if the source
    /// is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is propagated
    /// to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMap">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMap"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<TReturn> FlatMap<T, TReturn>(this Maybe<T> source, Func<T, Maybe<TReturn>> flatMap)
    {
        if (flatMap is null) throw new ArgumentNullException(nameof(flatMap));

        return source.Type switch
        {
            Some => flatMap(source.Value),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<TReturn>> FlatMapAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, CancellationToken, Task<Maybe<TReturn>>> flatMapAsync,
        CancellationToken cancellationToken)
    {
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        return source.Type switch
        {
            Some => await flatMapAsync(source.Value, cancellationToken),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static Task<Maybe<TReturn>> FlatMapAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, Task<Maybe<TReturn>>> flatMapAsync)
    {
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        return source.FlatMapAsync((value, _) => flatMapAsync(value), default);
    }

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result<T> Flatten<T>(this Result<Result<T>> source) =>
        source.FlatMap(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> source) =>
        source.FlatMap(nestedResult => nestedResult);

    /// <summary>
    /// Filter the specified result into a <c>None</c> result if it is a <c>Some</c> result and the
    /// <paramref name="filter"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="filter">
    /// A function that filters a <c>Some</c> result into a <c>None</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="filter"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> Filter<T>(this Maybe<T> source, Func<T, bool> filter)
    {
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        if (source.IsSome)
        {
            return filter(source.Value)
                ? source
                : Maybe<T>.Create.None();
        }

        return source;
    }

    /// <summary>
    /// Filter the specified result into a <c>None</c> result if it is a <c>Some</c> result and the
    /// <paramref name="filterAsync"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="filterAsync">
    /// A function that filters a <c>Some</c> result into a <c>None</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="filterAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<T>> FilterAsync<T>(
        this Maybe<T> source,
        Func<T, CancellationToken, Task<bool>> filterAsync,
        CancellationToken cancellationToken)
    {
        if (filterAsync is null) throw new ArgumentNullException(nameof(filterAsync));

        if (source.IsSome)
        {
            return await filterAsync(source.Value, cancellationToken)
                ? source
                : Maybe<T>.Create.None();
        }

        return source;
    }

    /// <summary>
    /// Filter the specified result into a <c>None</c> result if it is a <c>Some</c> result and the
    /// <paramref name="filterAsync"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="filterAsync">
    /// A function that filters a <c>Some</c> result into a <c>None</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="filterAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static Task<Maybe<T>> FilterAsync<T>(
        this Maybe<T> source,
        Func<T, Task<bool>> filterAsync)
    {
        if (filterAsync is null) throw new ArgumentNullException(nameof(filterAsync));

        return source.FilterAsync((value, _) => filterAsync(value), default);
    }
}
