using static RandomSkunk.Results.MaybeType;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>CrossMap</c> and <c>CrossMapAsync</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMap"/> is <see langword="null"/>.
    /// </exception>
    public static Result CrossMap<T>(
        this Result<T> source,
        Func<T, Result> crossMap,
        Func<Error, Error>? getError = null)
    {
        if (crossMap is null) throw new ArgumentNullException(nameof(crossMap));

        return source._type switch
        {
            Success => crossMap(source._value!),
            _ => Result.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result> CrossMapAsync<T>(
        this Result<T> source,
        Func<T, Task<Result>> crossMapAsync,
        Func<Error, Error>? getError = null)
    {
        if (crossMapAsync is null) throw new ArgumentNullException(nameof(crossMapAsync));

        return source._type switch
        {
            Success => await crossMapAsync(source._value!),
            _ => Result.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Maybe{T}"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMap"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<TReturn> CrossMap<T, TReturn>(
        this Result<T> source,
        Func<T, Maybe<TReturn>> crossMap,
        Func<Error, Error>? getError = null)
    {
        if (crossMap is null) throw new ArgumentNullException(nameof(crossMap));

        return source._type switch
        {
            Success => crossMap(source._value!),
            _ => Maybe<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Maybe{T}"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<TReturn>> CrossMapAsync<T, TReturn>(
        this Result<T> source,
        Func<T, Task<Maybe<TReturn>>> crossMapAsync,
        Func<Error, Error>? getError = null)
    {
        if (crossMapAsync is null) throw new ArgumentNullException(nameof(crossMapAsync));

        return source._type switch
        {
            Success => await crossMapAsync(source._value!),
            _ => Maybe<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultGetNoneError"/>
    /// is used instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMap"/> is <see langword="null"/>.
    /// </exception>
    public static Result CrossMap<T>(
        this Maybe<T> source,
        Func<T, Result> crossMap,
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null)
    {
        if (crossMap is null) throw new ArgumentNullException(nameof(crossMap));

        return source._type switch
        {
            Some => crossMap(source._value!),
            None => Result.Create.Fail(getNoneError.Evaluate()),
            _ => Result.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultGetNoneError"/>
    /// is used instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result> CrossMapAsync<T>(
        this Maybe<T> source,
        Func<T, Task<Result>> crossMapAsync,
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null)
    {
        if (crossMapAsync is null) throw new ArgumentNullException(nameof(crossMapAsync));

        return source._type switch
        {
            Some => await crossMapAsync(source._value!),
            None => Result.Create.Fail(getNoneError.Evaluate()),
            _ => Result.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result{T}"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultGetNoneError"/> is used
    /// instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMap"/> is <see langword="null"/>.
    /// </exception>
    public static Result<TReturn> CrossMap<T, TReturn>(
        this Maybe<T> source,
        Func<T, Result<TReturn>> crossMap,
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null)
    {
        if (crossMap is null) throw new ArgumentNullException(nameof(crossMap));

        return source._type switch
        {
            Some => crossMap(source._value!),
            None => Result<TReturn>.Create.Fail(getNoneError.Evaluate()),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result{T}"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultGetNoneError"/> is used
    /// instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If
    /// <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result<TReturn>> CrossMapAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, Task<Result<TReturn>>> crossMapAsync,
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null)
    {
        if (crossMapAsync is null) throw new ArgumentNullException(nameof(crossMapAsync));

        return source._type switch
        {
            Some => await crossMapAsync(source._value!),
            None => Result<TReturn>.Create.Fail(getNoneError.Evaluate()),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMap"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result> CrossMap<T>(
        this Task<Result<T>> source,
        Func<T, Result> crossMap,
        Func<Error, Error>? getError = null) =>
        (await source).CrossMap(crossMap, getError);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result> CrossMapAsync<T>(
        this Task<Result<T>> source,
        Func<T, Task<Result>> crossMapAsync,
        Func<Error, Error>? getError = null) =>
        await (await source).CrossMapAsync(crossMapAsync, getError);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Maybe{T}"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMap"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<TReturn>> CrossMap<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, Maybe<TReturn>> crossMap,
        Func<Error, Error>? getError = null) =>
        (await source).CrossMap(crossMap, getError);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Maybe{T}"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<TReturn>> CrossMapAsync<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, Task<Maybe<TReturn>>> crossMapAsync,
        Func<Error, Error>? getError = null) =>
        await (await source).CrossMapAsync(crossMapAsync, getError);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultGetNoneError"/>
    /// is used instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMap"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result> CrossMap<T>(
        this Task<Maybe<T>> source,
        Func<T, Result> crossMap,
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null) =>
        (await source).CrossMap(crossMap, getNoneError, getError);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultGetNoneError"/>
    /// is used instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result> CrossMapAsync<T>(
        this Task<Maybe<T>> source,
        Func<T, Task<Result>> crossMapAsync,
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null) =>
        await (await source).CrossMapAsync(crossMapAsync, getNoneError, getError);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result{T}"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultGetNoneError"/> is used
    /// instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If
    /// <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMap"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result<TReturn>> CrossMap<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, Result<TReturn>> crossMap,
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null) =>
        (await source).CrossMap(crossMap, getNoneError, getError);

    /// <summary>
    /// Maps <paramref name="source"/> to a <see cref="Result{T}"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="DefaultGetNoneError"/> is used
    /// instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If
    /// <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result<TReturn>> CrossMapAsync<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, Task<Result<TReturn>>> crossMapAsync,
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null) =>
        await (await source).CrossMapAsync(crossMapAsync, getNoneError, getError);
}
