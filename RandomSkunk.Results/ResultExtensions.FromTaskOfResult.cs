namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Else</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Converts this <see cref="Result{T}"/> to an equivalent <see cref="Maybe{T}"/>: if this is a <c>Success</c> result, then a
    /// <c>Some</c> result with the same value is returned; if this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned; a <c>None</c> result is never returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if this is a <c>Fail</c> result.
    /// </param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public static async Task<Maybe<T>> AsMaybe<T>(this Task<Result<T>> source, Func<Error, Error>? onFail = null) =>
        (await source.ConfigureAwait(false)).AsMaybe(onFail);

    /// <summary>
    /// Converts this <see cref="Maybe{T}"/> to an equivalent <see cref="Result{T}"/>: if this is a <c>Some</c> result, then a
    /// <c>Success</c> result with the same value is returned; if this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned; if this is a <c>None</c> result, then a <c>Fail</c> result with a "Not Found" error (error
    /// code: 404) is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onNone">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultOnNoneCallback"/> is used
    /// instead. Evaluated only if this is a <c>None</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if this is a <c>Fail</c> result.
    /// </param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public static async Task<Result<T>> AsResult<T>(
        this Task<Maybe<T>> source,
        Func<Error>? onNone = null,
        Func<Error, Error>? onFail = null) =>
        (await source.ConfigureAwait(false)).AsResult(onNone, onFail);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="onSuccess"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccess">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result> CrossMap<T>(
        this Task<Result<T>> source,
        Func<T, Result> onSuccess,
        Func<Error, Error>? onFail = null) =>
        (await source.ConfigureAwait(false)).CrossMap(onSuccess, onFail);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="onSuccessAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccessAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccessAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result> CrossMapAsync<T>(
        this Task<Result<T>> source,
        Func<T, Task<Result>> onSuccessAsync,
        Func<Error, Error>? onFail = null) =>
        await (await source.ConfigureAwait(false)).CrossMapAsync(onSuccessAsync, onFail).ConfigureAwait(false);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Maybe{T}"/> using the specified
    /// <paramref name="onSome"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSome">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSome"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<TReturn>> CrossMap<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, Maybe<TReturn>> onSome,
        Func<Error, Error>? onFail = null) =>
        (await source.ConfigureAwait(false)).CrossMap(onSome, onFail);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Maybe{T}"/> using the specified
    /// <paramref name="onSomeAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSomeAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSomeAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<TReturn>> CrossMapAsync<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, Task<Maybe<TReturn>>> onSomeAsync,
        Func<Error, Error>? onFail = null) =>
        await (await source.ConfigureAwait(false)).CrossMapAsync(onSomeAsync, onFail).ConfigureAwait(false);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="onSome"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="onNone"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSome">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="onNone">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultOnNoneCallback"/>
    /// is used instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSome"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result> CrossMap<T>(
        this Task<Maybe<T>> source,
        Func<T, Result> onSome,
        Func<Error>? onNone = null,
        Func<Error, Error>? onFail = null) =>
        (await source.ConfigureAwait(false)).CrossMap(onSome, onNone, onFail);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="onSomeAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="onNone"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSomeAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="onNone">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultOnNoneCallback"/>
    /// is used instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSomeAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result> CrossMapAsync<T>(
        this Task<Maybe<T>> source,
        Func<T, Task<Result>> onSomeAsync,
        Func<Error>? onNone = null,
        Func<Error, Error>? onFail = null) =>
        await (await source.ConfigureAwait(false)).CrossMapAsync(onSomeAsync, onNone, onFail).ConfigureAwait(false);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result{T}"/> using the specified
    /// <paramref name="onSome"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="onNone"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSome">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="onNone">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultOnNoneCallback"/> is used
    /// instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If
    /// <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSome"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result<TReturn>> CrossMap<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, Result<TReturn>> onSome,
        Func<Error>? onNone = null,
        Func<Error, Error>? onFail = null) =>
        (await source.ConfigureAwait(false)).CrossMap(onSome, onNone, onFail);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result{T}"/> using the specified
    /// <paramref name="onSomeAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="onNone"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSomeAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="onNone">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultOnNoneCallback"/> is used
    /// instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If
    /// <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSomeAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result<TReturn>> CrossMapAsync<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, Task<Result<TReturn>>> onSomeAsync,
        Func<Error>? onNone = null,
        Func<Error, Error>? onFail = null) =>
        await (await source.ConfigureAwait(false)).CrossMapAsync(onSomeAsync, onNone, onFail).ConfigureAwait(false);

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="source"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Result> Else(this Task<Result> source, Result fallbackResult) =>
        (await source.ConfigureAwait(false)).Else(fallbackResult);

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
    public static async Task<Result> Else(this Task<Result> source, Func<Result> getFallbackResult) =>
        (await source.ConfigureAwait(false)).Else(getFallbackResult);

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="source"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Result<T>> Else<T>(this Task<Result<T>> source, Result<T> fallbackResult) =>
        (await source.ConfigureAwait(false)).Else(fallbackResult);

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
    public static async Task<Result<T>> Else<T>(this Task<Result<T>> source, Func<Result<T>> getFallbackResult) =>
        (await source.ConfigureAwait(false)).Else(getFallbackResult);

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Some</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Some</c>.</param>
    /// <returns>Either <paramref name="source"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Maybe<T>> Else<T>(this Task<Maybe<T>> source, Maybe<T> fallbackResult) =>
        (await source.ConfigureAwait(false)).Else(fallbackResult);

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
    public static async Task<Maybe<T>> Else<T>(this Task<Maybe<T>> source, Func<Maybe<T>> getFallbackResult) =>
        (await source.ConfigureAwait(false)).Else(getFallbackResult);

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
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="filter"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<T>> Filter<T>(
        this Task<Maybe<T>> source,
        Func<T, bool> filter) =>
        (await source.ConfigureAwait(false)).Filter(filter);

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
    public static async Task<Maybe<T>> FilterAsync<T>(
        this Task<Maybe<T>> source,
        Func<T, Task<bool>> filterAsync) =>
        await (await source.ConfigureAwait(false)).FilterAsync(filterAsync).ConfigureAwait(false);

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="onSuccess"/> function. The flat map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccess">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result<TReturn>> FlatMap<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, Result<TReturn>> onSuccess,
        Func<Error, Error>? onFail = null) =>
        (await source.ConfigureAwait(false)).FlatMap(onSuccess, onFail);

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="onSuccessAsync"/> function. The flat map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccessAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccessAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result<TReturn>> FlatMapAsync<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, Task<Result<TReturn>>> onSuccessAsync,
        Func<Error, Error>? onFail = null) =>
        await (await source.ConfigureAwait(false)).FlatMapAsync(onSuccessAsync, onFail).ConfigureAwait(false);

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="onSome"/> function. The flat map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, a
    /// <c>None</c> result is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSome">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSome"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<TReturn>> FlatMap<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, Maybe<TReturn>> onSome,
        Func<Error, Error>? onFail = null) =>
        (await source.ConfigureAwait(false)).FlatMap(onSome, onFail);

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="onSomeAsync"/> function. The flat map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, a
    /// <c>None</c> result is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSomeAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSomeAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<TReturn>> FlatMapAsync<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, Task<Maybe<TReturn>>> onSomeAsync,
        Func<Error, Error>? onFail = null) =>
        await (await source.ConfigureAwait(false)).FlatMapAsync(onSomeAsync, onFail).ConfigureAwait(false);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Result<T>> Flatten<T>(this Task<Result<Result<T>>> source) =>
        (await source.ConfigureAwait(false)).Flatten();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Maybe<T>> Flatten<T>(this Task<Maybe<Maybe<T>>> source) =>
        (await source.ConfigureAwait(false)).Flatten();

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
    public static async Task<T?> GetValueOr<T>(this Task<Result<T>> source, T? fallbackValue) =>
        (await source.ConfigureAwait(false)).GetValueOr(fallbackValue);

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
    public static async Task<T?> GetValueOr<T>(this Task<Result<T>> source, Func<T?> getFallbackValue) =>
        (await source.ConfigureAwait(false)).GetValueOr(getFallbackValue);

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
    public static async Task<T?> GetValueOr<T>(this Task<Maybe<T>> source, T? fallbackValue) =>
        (await source.ConfigureAwait(false)).GetValueOr(fallbackValue);

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
    public static async Task<T?> GetValueOr<T>(this Task<Maybe<T>> source, Func<T?> getFallbackValue) =>
        (await source.ConfigureAwait(false)).GetValueOr(getFallbackValue);

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="onSuccess"/>
    /// function. The map function is evaluated if and only if the source is a <c>Success</c>
    /// result, and the <see cref="Result{T}.Type"/> of the new result will always be the same as
    /// the source result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccess">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="onSuccess"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<Result<TReturn>> Map<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, TReturn> onSuccess,
        Func<Error, Error>? onFail = null) =>
        (await source.ConfigureAwait(false)).Map(onSuccess, onFail);

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="onSuccessAsync"/>
    /// function. The map function is evaluated if and only if the source is a <c>Success</c>
    /// result, and the <see cref="Result{T}.Type"/> of the new result will always be the same as
    /// the source result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccessAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccessAsync"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="onSuccessAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<Result<TReturn>> MapAsync<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, Task<TReturn>> onSuccessAsync,
        Func<Error, Error>? onFail = null) =>
        await (await source.ConfigureAwait(false)).MapAsync(onSuccessAsync, onFail).ConfigureAwait(false);

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="onSome"/>
    /// function. The map function is evaluated if and only if the source is a <c>Some</c> result,
    /// and the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSome">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSome"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="onSome"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<Maybe<TReturn>> Map<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, TReturn> onSome,
        Func<Error, Error>? onFail = null) =>
        (await source.ConfigureAwait(false)).Map(onSome, onFail);

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="onSomeAsync"/>
    /// function. The map function is evaluated if and only if the source is a <c>Some</c> result,
    /// and the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSomeAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSomeAsync"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="onSomeAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<Maybe<TReturn>> MapAsync<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, Task<TReturn>> onSomeAsync,
        Func<Error, Error>? onFail = null) =>
        await (await source.ConfigureAwait(false)).MapAsync(onSomeAsync, onFail).ConfigureAwait(false);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<T> Match<T>(
        this Task<Result> source,
        Func<T> onSuccess,
        Func<Error, T> onFail) =>
        (await source.ConfigureAwait(false)).Match(onSuccess, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task that represents the match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task Match(
        this Task<Result> source,
        Action onSuccess,
        Action<Error> onFail) =>
        (await source.ConfigureAwait(false)).Match(onSuccess, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<T> MatchAsync<T>(
        this Task<Result> source,
        Func<Task<T>> onSuccess,
        Func<Error, Task<T>> onFail) =>
        await (await source.ConfigureAwait(false)).MatchAsync(onSuccess, onFail).ConfigureAwait(false);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task MatchAsync(
        this Task<Result> source,
        Func<Task> onSuccess,
        Func<Error, Task> onFail) =>
        await (await source.ConfigureAwait(false)).MatchAsync(onSuccess, onFail).ConfigureAwait(false);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, TReturn> onSuccess,
        Func<Error, TReturn> onFail) =>
            (await source.ConfigureAwait(false)).Match(onSuccess, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task that represents the match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task Match<T>(
        this Task<Result<T>> source,
        Action<T> onSuccess,
        Action<Error> onFail) =>
            (await source.ConfigureAwait(false)).Match(onSuccess, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<TReturn> MatchAsync<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFail) =>
            await (await source.ConfigureAwait(false)).MatchAsync(onSuccess, onFail).ConfigureAwait(false);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>Success</c>. The non-null value of the
    /// <c>Success</c> result is passed to this function.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/> or if <paramref name="onFail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task MatchAsync<T>(
        this Task<Result<T>> source,
        Func<T, Task> onSuccess,
        Func<Error, Task> onFail) =>
            await (await source.ConfigureAwait(false)).MatchAsync(onSuccess, onFail).ConfigureAwait(false);

    /// <summary>
    /// Evaluates either the <paramref name="onSome"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSome">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSome"/> is <see langword="null"/>, or if <paramref name="onNone"/> is
    /// <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, TReturn> onSome,
        Func<TReturn> onNone,
        Func<Error, TReturn> onFail) =>
        (await source.ConfigureAwait(false)).Match(onSome, onNone, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSome"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSome">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task that represents the match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSome"/> is <see langword="null"/>, or if <paramref name="onNone"/> is
    /// <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    /// </exception>
    public static async Task Match<T>(
        this Task<Maybe<T>> source,
        Action<T> onSome,
        Action onNone,
        Action<Error> onFail) =>
        (await source.ConfigureAwait(false)).Match(onSome, onNone, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSome"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="onSome">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSome"/> is <see langword="null"/>, or if <paramref name="onNone"/> is
    /// <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<TReturn> MatchAsync<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, Task<TReturn>> onSome,
        Func<Task<TReturn>> onNone,
        Func<Error, Task<TReturn>> onFail) =>
        await (await source.ConfigureAwait(false)).MatchAsync(onSome, onNone, onFail).ConfigureAwait(false);

    /// <summary>
    /// Evaluates either the <paramref name="noneome"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="noneome">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="noneome"/> is <see langword="null"/>, or if <paramref name="onNone"/> is
    /// <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    /// </exception>
    public static async Task MatchAsync<T>(
        this Task<Maybe<T>> source,
        Func<T, Task> noneome,
        Func<Task> onNone,
        Func<Error, Task> onFail) =>
        await (await source.ConfigureAwait(false)).MatchAsync(noneome, onNone, onFail).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Result> OnFail(this Task<Result> source, Action<Error> onFail) =>
        (await source.ConfigureAwait(false)).OnFail(onFail);

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Result> OnFailAsync(this Task<Result> source, Func<Error, Task> onFail) =>
        await (await source.ConfigureAwait(false)).OnFailAsync(onFail).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Result<T>> OnFail<T>(this Task<Result<T>> source, Action<Error> onFail) =>
        (await source.ConfigureAwait(false)).OnFail(onFail);

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Result<T>> OnFailAsync<T>(this Task<Result<T>> source, Func<Error, Task> onFail) =>
        await (await source.ConfigureAwait(false)).OnFailAsync(onFail).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnFail<T>(this Task<Maybe<T>> source, Action<Error> onFail) =>
        (await source.ConfigureAwait(false)).OnFail(onFail);

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnFailAsync<T>(this Task<Maybe<T>> source, Func<Error, Task> onFail) =>
        await (await source.ConfigureAwait(false)).OnFailAsync(onFail).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onNone"/> function if <paramref name="source"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onNone">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnNone<T>(this Task<Maybe<T>> source, Action onNone) =>
        (await source.ConfigureAwait(false)).OnNone(onNone);

    /// <summary>
    /// Invokes the <paramref name="onNone"/> function if <paramref name="source"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onNone">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnNoneAsync<T>(this Task<Maybe<T>> source, Func<Task> onNone) =>
        await (await source.ConfigureAwait(false)).OnNoneAsync(onNone).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onSome"/> function if <paramref name="source"/> is a <c>Some</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onSome">A callback function to invoke if the source is a <c>Some</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnSome<T>(this Task<Maybe<T>> source, Action<T> onSome) =>
        (await source.ConfigureAwait(false)).OnSome(onSome);

    /// <summary>
    /// Invokes the <paramref name="onSome"/> function if <paramref name="source"/> is a <c>Some</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onSome">A callback function to invoke if the source is a <c>Some</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnSomeAsync<T>(this Task<Maybe<T>> source, Func<T, Task> onSome) =>
        await (await source.ConfigureAwait(false)).OnSomeAsync(onSome).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onSuccess"/> function if <paramref name="source"/> is a <c>Success</c> result.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="onSuccess">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Result> OnSuccess(this Task<Result> source, Action onSuccess) =>
        (await source.ConfigureAwait(false)).OnSuccess(onSuccess);

    /// <summary>
    /// Invokes the <paramref name="onSuccess"/> function if <paramref name="source"/> is a <c>Success</c> result.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="onSuccess">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Result> OnSuccessAsync(this Task<Result> source, Func<Task> onSuccess) =>
        await (await source.ConfigureAwait(false)).OnSuccessAsync(onSuccess).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onSuccess"/> function if <paramref name="source"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onSuccess">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> source, Action<T> onSuccess) =>
        (await source.ConfigureAwait(false)).OnSuccess(onSuccess);

    /// <summary>
    /// Invokes the <paramref name="onSuccess"/> function if <paramref name="source"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onSuccess">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Result<T>> OnSuccessAsync<T>(this Task<Result<T>> source, Func<T, Task> onSuccess) =>
        await (await source.ConfigureAwait(false)).OnSuccessAsync(onSuccess).ConfigureAwait(false);

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
    public static async Task<Result<T>> Or<T>(this Task<Result<T>> source, [DisallowNull] T fallbackValue) =>
        (await source.ConfigureAwait(false)).Or(fallbackValue);

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
    public static async Task<Result<T>> Or<T>(this Task<Result<T>> source, Func<T> getFallbackValue) =>
        (await source.ConfigureAwait(false)).Or(getFallbackValue);

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
    public static async Task<Maybe<T>> Or<T>(this Task<Maybe<T>> source, [DisallowNull] T fallbackValue) =>
        (await source.ConfigureAwait(false)).Or(fallbackValue);

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
    public static async Task<Maybe<T>> Or<T>(this Task<Maybe<T>> source, Func<T> getFallbackValue) =>
        (await source.ConfigureAwait(false)).Or(getFallbackValue);

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="getError">
    /// A function that returns the error for the returned <c>Fail</c> result.
    /// </param>
    /// <returns>
    /// A new <c>Fail</c> result with its error specified by the <paramref name="getError"/>
    /// function if this is a <c>Fail</c> result; otherwise, <paramref name="source"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getError"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result> WithError(this Task<Result> source, Func<Error, Error> getError) =>
        (await source.ConfigureAwait(false)).WithError(getError);

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getError">
    /// A function that returns the error for the returned <c>Fail</c> result.
    /// </param>
    /// <returns>
    /// A new <c>Fail</c> result with its error specified by the <paramref name="getError"/>
    /// function if this is a <c>Fail</c> result; otherwise, <paramref name="source"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getError"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result<T>> WithError<T>(this Task<Result<T>> source, Func<Error, Error> getError) =>
        (await source.ConfigureAwait(false)).WithError(getError);

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getError">
    /// A function that returns the error for the returned <c>Fail</c> result.
    /// </param>
    /// <returns>
    /// A new <c>Fail</c> result with its error specified by the <paramref name="getError"/>
    /// function if this is a <c>Fail</c> result; otherwise, <paramref name="source"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getError"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<T>> WithError<T>(this Task<Maybe<T>> source, Func<Error, Error> getError) =>
        (await source.ConfigureAwait(false)).WithError(getError);
}
