using static RandomSkunk.Results.MaybeType;

namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static partial class ResultExtensions
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
}
